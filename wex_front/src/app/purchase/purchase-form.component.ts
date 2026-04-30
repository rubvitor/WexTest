import { Component, inject } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { PurchaseApiService } from '../api/purchase-api.service';

@Component({
  selector: 'app-purchase-form',
  standalone: true,
  imports: [ReactiveFormsModule],
  template: `
    <section class="card">
      <h2>Store Purchase</h2>

      <form [formGroup]="form" (ngSubmit)="submit()">
        <label>
          Description
          <input formControlName="description" />
        </label>

        <label>
          Transaction Date
          <input type="date" formControlName="transactionDate" />
        </label>

        <label>
          Purchase Amount USD
          <input type="number" formControlName="purchaseAmountUsd" />
        </label>

        <button type="submit" [disabled]="form.invalid || loading">
          Store Purchase
        </button>
      </form>

      @if (message) {
        <p>{{ message }}</p>
      }

      @if (error) {
        <p class="error">{{ error }}</p>
      }
    </section>
  `
})
export class PurchaseFormComponent {
  private readonly fb = inject(FormBuilder);
  private readonly api = inject(PurchaseApiService);

  loading = false;
  message = '';
  error = '';

  readonly form = this.fb.nonNullable.group({
    description: ['', Validators.required],
    transactionDate: ['', Validators.required],
    purchaseAmountUsd: [0, [Validators.required, Validators.min(0.01)]]
  });

  submit(): void {
    if (this.form.invalid) {
      return;
    }

    this.loading = true;
    this.message = '';
    this.error = '';

    this.api.create(this.form.getRawValue()).subscribe({
      next: () => {
        this.message = 'Purchase stored successfully.';
        this.loading = false;

        this.form.reset({
          description: '',
          transactionDate: '',
          purchaseAmountUsd: 0
        });
      },
      error: err => {
        this.error = err?.error?.message ?? 'Unable to store purchase.';
        this.loading = false;
      }
    });
  }
}