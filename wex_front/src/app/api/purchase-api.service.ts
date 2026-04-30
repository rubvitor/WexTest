import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { PurchaseResponse, StorePurchaseRequest } from './models';

@Injectable({ providedIn: 'root' })
export class PurchaseApiService {
  constructor(private readonly http: HttpClient) {}
  create(request: StorePurchaseRequest) { return this.http.post<PurchaseResponse>(`${environment.purchaseApiUrl}/api/purchases`, request); }
}
