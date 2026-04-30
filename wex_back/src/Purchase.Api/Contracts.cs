namespace Wex.Purchase.Api;

public sealed record StorePurchaseRequest(string Description, DateOnly TransactionDate, decimal PurchaseAmountUsd);
public sealed record PurchaseResponse(Guid Id, string Description, DateOnly TransactionDate, decimal PurchaseAmountUsd);
