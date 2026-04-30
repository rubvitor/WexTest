namespace Wex.SharedKernel;

public interface IDomainEvent
{
    Guid EventId { get; }
    DateTimeOffset OccurredAt { get; }
}

public sealed record PurchaseStoredEvent(
    Guid EventId,
    DateTimeOffset OccurredAt,
    Guid PurchaseId,
    string Description,
    DateOnly TransactionDate,
    decimal AmountUsd) : IDomainEvent;
