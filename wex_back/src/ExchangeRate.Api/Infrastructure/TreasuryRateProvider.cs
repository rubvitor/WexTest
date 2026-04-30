using System.Globalization;
using System.Net.Http.Json;
using System.Text.Json;
using Wex.ExchangeRate.Api.Application;

namespace Wex.ExchangeRate.Api.Infrastructure;

public sealed class TreasuryRateProvider(HttpClient httpClient, IWebHostEnvironment env) : IExchangeRateProvider
{
    private readonly string _cacheFile = Path.Combine(env.ContentRootPath, "data", "rates-cache.json");
    private readonly string _seedFile = Path.Combine(env.ContentRootPath, "Seed", "treasury-rates.sample.json");
    private readonly JsonSerializerOptions _json = new(JsonSerializerDefaults.Web);

    public async Task<IReadOnlyCollection<TreasuryRate>> GetRatesAsync(CancellationToken cancellationToken)
    {
        try
        {
            var url = "https://api.fiscaldata.treasury.gov/services/api/fiscal_service/v1/accounting/od/rates_of_exchange?fields=country,currency,exchange_rate,record_date&page[size]=10000&sort=-record_date";
            var document = await httpClient.GetFromJsonAsync<TreasuryResponse>(url, cancellationToken);
            var rates = document?.Data.Select(ToRate).Where(x => x is not null).Select(x => x!).ToArray() ?? [];
            if (rates.Length > 0)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(_cacheFile)!);
                await File.WriteAllTextAsync(_cacheFile, JsonSerializer.Serialize(rates, _json), cancellationToken);
                return rates;
            }
        }
        catch { /* Offline fallback keeps the project plug and play. */ }

        var fallbackPath = File.Exists(_cacheFile) ? _cacheFile : _seedFile;
        return JsonSerializer.Deserialize<TreasuryRate[]>(await File.ReadAllTextAsync(fallbackPath, cancellationToken), _json) ?? [];
    }

    private static TreasuryRate? ToRate(TreasuryRow row)
    {
        if (!decimal.TryParse(row.ExchangeRate, NumberStyles.Any, CultureInfo.InvariantCulture, out var rate)) return null;
        if (!DateOnly.TryParse(row.RecordDate, CultureInfo.InvariantCulture, out var date)) return null;
        return new TreasuryRate(row.Country, row.Currency, date, rate);
    }

    private sealed record TreasuryResponse(TreasuryRow[] Data);
    private sealed record TreasuryRow(string Country, string Currency, string ExchangeRate, string RecordDate);
}
