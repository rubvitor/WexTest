using FluentAssertions;
using Wex.Purchase.Api.Domain;

namespace Wex.Tests;

public sealed class PurchaseTests
{
    [Fact]
    public void Create_rejects_description_longer_than_50_characters()
    {
        var result = PurchaseTransaction.Create(new string('a', 51), new DateOnly(2026, 1, 1), 10);
        result.IsSuccess.Should().BeFalse();
        result.Error!.Code.Should().Be("DescriptionTooLong");
    }

    [Fact]
    public void Create_rounds_purchase_amount_to_nearest_cent()
    {
        var result = PurchaseTransaction.Create("Fuel", new DateOnly(2026, 1, 1), 10.555m);
        result.Value!.Amount.Amount.Should().Be(10.56m);
    }
}
