using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wex.Purchase.Api.Infrastructure.Persistence.Entities;

namespace Wex.Purchase.Api.Infrastructure.Persistence.Configurations;

public sealed class PurchaseTransactionEntityConfiguration : IEntityTypeConfiguration<PurchaseTransactionEntity>
{
    public void Configure(EntityTypeBuilder<PurchaseTransactionEntity> builder)
    {
        builder.ToTable("PurchaseTransactions");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Description)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.TransactionDate)
            .IsRequired();

        builder.Property(x => x.PurchaseAmountUsd)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.CreatedAtUtc)
            .IsRequired();

        builder.HasIndex(x => x.TransactionDate);
    }
}
