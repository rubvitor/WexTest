namespace Wex.ExchangeRate.Api;

public sealed record PurchaseDto(Guid Id, string Description, DateOnly TransactionDate, decimal PurchaseAmountUsd);
public sealed record ConversionResponse(Guid Id, string Description, DateOnly TransactionDate, decimal OriginalAmountUsd, string Country, string Currency, decimal ExchangeRate, decimal ConvertedAmount);
public sealed record TreasuryRate(string Country, string Currency, DateOnly EffectiveDate, decimal ExchangeRate);
