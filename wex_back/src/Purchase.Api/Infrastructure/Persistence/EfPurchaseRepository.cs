using Microsoft.EntityFrameworkCore;
using Wex.Purchase.Api.Application;
using Wex.Purchase.Api.Domain;
using Wex.Purchase.Api.Infrastructure.Persistence.Entities;

namespace Wex.Purchase.Api.Infrastructure.Persistence;

public sealed class EfPurchaseRepository(PurchaseDbContext dbContext) : IPurchaseRepository
{
    public async Task SaveAsync(PurchaseTransaction purchase, CancellationToken cancellationToken)
    {
        dbContext.PurchaseTransactions.Add(purchase.ToEntity());
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<PurchaseTransaction?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => await dbContext.PurchaseTransactions
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => x.ToDomain())
            .SingleOrDefaultAsync(cancellationToken);

    public async Task<IReadOnlyCollection<PurchaseTransaction>> ListAsync(CancellationToken cancellationToken)
        => await dbContext.PurchaseTransactions
            .AsNoTracking()
            .OrderByDescending(x => x.TransactionDate)
            .ThenBy(x => x.Description)
            .Select(x => x.ToDomain())
            .ToArrayAsync(cancellationToken);
}

internal static class PurchasePersistenceMappings
{
    public static PurchaseTransactionEntity ToEntity(this PurchaseTransaction purchase)
        => new()
        {
            Id = purchase.Id,
            Description = purchase.Description,
            TransactionDate = purchase.TransactionDate,
            PurchaseAmountUsd = purchase.Amount.Amount,
            CreatedAtUtc = DateTimeOffset.UtcNow
        };

    public static PurchaseTransaction ToDomain(this PurchaseTransactionEntity entity)
        => PurchaseTransaction.Rehydrate(entity.Id, entity.Description, entity.TransactionDate, entity.PurchaseAmountUsd);
}
