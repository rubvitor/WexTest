using Wex.SharedKernel;

namespace Wex.ExchangeRate.Api.Application;

public interface IPurchaseClient { Task<PurchaseDto?> GetAsync(Guid id, CancellationToken cancellationToken); }
public interface IExchangeRateProvider { Task<IReadOnlyCollection<TreasuryRate>> GetRatesAsync(CancellationToken cancellationToken); }

public sealed class ExchangeRateService(IPurchaseClient purchaseClient, IExchangeRateProvider rateProvider)
{
    public async Task<Result<ConversionResponse>> ConvertAsync(Guid purchaseId, string country, CancellationToken cancellationToken)
    {
        var purchase = await purchaseClient.GetAsync(purchaseId, cancellationToken);
        if (purchase is null) return Result<ConversionResponse>.Failure("PurchaseNotFound", "Purchase transaction was not found.");

        var cutoff = purchase.TransactionDate.AddMonths(-6);
        var rate = (await rateProvider.GetRatesAsync(cancellationToken))
            .Where(x => x.Country.Equals(country, StringComparison.OrdinalIgnoreCase))
            .Where(x => x.EffectiveDate <= purchase.TransactionDate && x.EffectiveDate >= cutoff)
            .OrderByDescending(x => x.EffectiveDate)
            .FirstOrDefault();

        if (rate is null)
            return Result<ConversionResponse>.Failure("RateNotFound", "The purchase cannot be converted to the target currency because no exchange rate exists within six months on or before the purchase date.");

        var converted = Money.Usd(purchase.PurchaseAmountUsd).Convert(rate.ExchangeRate, rate.Currency);
        return Result<ConversionResponse>.Success(new ConversionResponse(purchase.Id, purchase.Description, purchase.TransactionDate, purchase.PurchaseAmountUsd, rate.Country, rate.Currency, rate.ExchangeRate, converted.Amount));
    }
}
