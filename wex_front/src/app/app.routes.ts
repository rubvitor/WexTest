import { Routes } from '@angular/router';
import { PurchaseFormComponent } from './purchase/purchase-form.component';
import { ConversionComponent } from './conversion/conversion.component';

export const routes: Routes = [
  { path: '', redirectTo: 'purchase', pathMatch: 'full' },
  { path: 'purchase', component: PurchaseFormComponent },
  { path: 'conversion', component: ConversionComponent },
  { path: '**', redirectTo: 'purchase' }
];