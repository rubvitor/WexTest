namespace Wex.Purchase.Api.Infrastructure.Persistence.Entities;

public sealed class PurchaseTransactionEntity
{
    public Guid Id { get; init; }
    public string Description { get; init; } = string.Empty;
    public DateOnly TransactionDate { get; init; }
    public decimal PurchaseAmountUsd { get; init; }
    public DateTimeOffset CreatedAtUtc { get; init; }
}
