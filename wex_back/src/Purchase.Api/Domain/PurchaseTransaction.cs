using Wex.SharedKernel;

namespace Wex.Purchase.Api.Domain;

public sealed class PurchaseTransaction
{
    private PurchaseTransaction(Guid id, string description, DateOnly transactionDate, Money amount)
    {
        Id = id;
        Description = description;
        TransactionDate = transactionDate;
        Amount = amount;
    }

    public Guid Id { get; }
    public string Description { get; }
    public DateOnly TransactionDate { get; }
    public Money Amount { get; }

    public static Result<PurchaseTransaction> Create(string description, DateOnly transactionDate, decimal purchaseAmountUsd)
    {
        if (string.IsNullOrWhiteSpace(description)) return Result<PurchaseTransaction>.Failure("DescriptionRequired", "Description is required.");
        if (description.Length > 50) return Result<PurchaseTransaction>.Failure("DescriptionTooLong", "Description must not exceed 50 characters.");
        if (purchaseAmountUsd <= 0) return Result<PurchaseTransaction>.Failure("InvalidAmount", "Purchase amount must be a positive value.");

        return Result<PurchaseTransaction>.Success(new PurchaseTransaction(Guid.NewGuid(), description.Trim(), transactionDate, Money.Usd(purchaseAmountUsd)));
    }

    internal static PurchaseTransaction Rehydrate(Guid id, string description, DateOnly transactionDate, decimal purchaseAmountUsd)
        => new(id, description, transactionDate, Money.Usd(purchaseAmountUsd));
}
