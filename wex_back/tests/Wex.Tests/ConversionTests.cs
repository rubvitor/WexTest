using FluentAssertions;
using Wex.ExchangeRate.Api;
using Wex.ExchangeRate.Api.Application;

namespace Wex.Tests;

public sealed class ConversionTests
{
    [Fact]
    public async Task Convert_uses_latest_rate_on_or_before_purchase_date_within_six_months()
    {
        var service = new ExchangeRateService(
            new FakePurchaseClient(new PurchaseDto(Guid.NewGuid(), "Fuel", new DateOnly(2026, 4, 1), 10.00m)),
            new FakeRateProvider([
                new TreasuryRate("Brazil", "Real", new DateOnly(2026, 1, 1), 5.00m),
                new TreasuryRate("Brazil", "Real", new DateOnly(2026, 3, 31), 5.25m),
                new TreasuryRate("Brazil", "Real", new DateOnly(2026, 4, 2), 99m)
            ]));

        var result = await service.ConvertAsync(Guid.NewGuid(), "Brazil", CancellationToken.None);

        result.Value!.ExchangeRate.Should().Be(5.25m);
        result.Value.ConvertedAmount.Should().Be(52.50m);
    }

    [Fact]
    public async Task Convert_returns_error_when_rate_is_older_than_six_months()
    {
        var service = new ExchangeRateService(
            new FakePurchaseClient(new PurchaseDto(Guid.NewGuid(), "Fuel", new DateOnly(2026, 4, 1), 10.00m)),
            new FakeRateProvider([new TreasuryRate("Brazil", "Real", new DateOnly(2025, 1, 1), 5.00m)]));

        var result = await service.ConvertAsync(Guid.NewGuid(), "Brazil", CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error!.Code.Should().Be("RateNotFound");
    }

    private sealed class FakePurchaseClient(PurchaseDto purchase) : IPurchaseClient
    {
        public Task<PurchaseDto?> GetAsync(Guid id, CancellationToken cancellationToken) => Task.FromResult<PurchaseDto?>(purchase);
    }

    private sealed class FakeRateProvider(IReadOnlyCollection<TreasuryRate> rates) : IExchangeRateProvider
    {
        public Task<IReadOnlyCollection<TreasuryRate>> GetRatesAsync(CancellationToken cancellationToken) => Task.FromResult(rates);
    }
}
