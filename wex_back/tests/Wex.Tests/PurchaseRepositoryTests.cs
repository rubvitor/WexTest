using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Wex.Purchase.Api.Domain;
using Wex.Purchase.Api.Infrastructure.Persistence;

namespace Wex.Tests;

public sealed class PurchaseRepositoryTests
{
    [Fact]
    public async Task Save_and_get_purchase_uses_ef_core_in_memory_with_linq_to_entities()
    {
        await using var dbContext = CreateContext();
        var repository = new EfPurchaseRepository(dbContext);
        var purchase = PurchaseTransaction.Create("Hotel", new DateOnly(2026, 4, 29), 123.456m).Value!;

        await repository.SaveAsync(purchase, CancellationToken.None);
        var stored = await repository.GetByIdAsync(purchase.Id, CancellationToken.None);

        stored.Should().NotBeNull();
        stored!.Id.Should().Be(purchase.Id);
        stored.Description.Should().Be("Hotel");
        stored.Amount.Amount.Should().Be(123.46m);
    }

    [Fact]
    public async Task List_orders_purchases_using_linq_to_entities()
    {
        await using var dbContext = CreateContext();
        var repository = new EfPurchaseRepository(dbContext);

        await repository.SaveAsync(PurchaseTransaction.Create("Old", new DateOnly(2026, 1, 1), 10m).Value!, CancellationToken.None);
        await repository.SaveAsync(PurchaseTransaction.Create("New", new DateOnly(2026, 2, 1), 20m).Value!, CancellationToken.None);

        var purchases = await repository.ListAsync(CancellationToken.None);

        purchases.Select(x => x.Description).Should().ContainInOrder("New", "Old");
    }

    private static PurchaseDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<PurchaseDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString("N"))
            .Options;

        return new PurchaseDbContext(options);
    }
}
