import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { ConversionResponse } from './models';

@Injectable({ providedIn: 'root' })
export class ConversionApiService {
  constructor(private readonly http: HttpClient) {}
  convert(purchaseId: string, country: string) {
    const params = new HttpParams().set('country', country);
    return this.http.get<ConversionResponse>(`${environment.exchangeApiUrl}/api/conversions/${purchaseId}`, { params });
  }
}
