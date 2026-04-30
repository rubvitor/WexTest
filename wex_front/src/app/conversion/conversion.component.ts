import { Component, inject } from '@angular/core';
import { DecimalPipe } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { ConversionApiService } from '../api/conversion-api.service';
import { ConversionResponse } from '../api/models';

@Component({
  selector: 'app-conversion',
  standalone: true,
  imports: [ReactiveFormsModule, DecimalPipe],
  template: `
    <section class="card">
      <h2>Convert Purchase</h2>

      <form [formGroup]="form" (ngSubmit)="submit()">
        <label>
          Purchase Id
          <input formControlName="purchaseId" />
        </label>

        <label>
          Country
          <input formControlName="country" placeholder="Brazil" />
        </label>

        <button type="submit" [disabled]="form.invalid || loading">
          Convert
        </button>
      </form>

      @if (conversion) {
        <div class="result">
          <p>
            <strong>{{ conversion.description }}</strong>
            —
            {{ conversion.transactionDate }}
          </p>

          <p>
            USD {{ conversion.originalAmountUsd | number:'1.2-2' }}
            × {{ conversion.exchangeRate }}
            =
            {{ conversion.currency }}
            {{ conversion.convertedAmount | number:'1.2-2' }}
          </p>
        </div>
      }

      @if (error) {
        <p class="error">{{ error }}</p>
      }
    </section>
  `
})
export class ConversionComponent {
  private readonly fb = inject(FormBuilder);
  private readonly api = inject(ConversionApiService);

  loading = false;
  conversion?: ConversionResponse;
  error = '';

  readonly form = this.fb.nonNullable.group({
    purchaseId: ['', Validators.required],
    country: ['Brazil', Validators.required]
  });

  submit(): void {
    if (this.form.invalid) {
      return;
    }

    this.loading = true;
    this.error = '';
    this.conversion = undefined;

    const value = this.form.getRawValue();

    this.api.convert(value.purchaseId, value.country).subscribe({
      next: conversion => {
        this.conversion = conversion;
        this.loading = false;
      },
      error: err => {
        this.error = err?.error?.message ?? 'Unable to convert purchase.';
        this.loading = false;
      }
    });
  }
}