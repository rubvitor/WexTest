using Microsoft.EntityFrameworkCore;
using Wex.Purchase.Api.Infrastructure.Persistence.Entities;

namespace Wex.Purchase.Api.Infrastructure.Persistence;

public sealed class PurchaseDbContext(DbContextOptions<PurchaseDbContext> options) : DbContext(options)
{
    public DbSet<PurchaseTransactionEntity> PurchaseTransactions => Set<PurchaseTransactionEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new Configurations.PurchaseTransactionEntityConfiguration());
    }
}
