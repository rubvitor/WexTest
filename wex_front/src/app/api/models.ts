export interface StorePurchaseRequest { description: string; transactionDate: string; purchaseAmountUsd: number; }
export interface PurchaseResponse extends StorePurchaseRequest { id: string; }
export interface ConversionResponse { id: string; description: string; transactionDate: string; originalAmountUsd: number; country: string; currency: string; exchangeRate: number; convertedAmount: number; }
