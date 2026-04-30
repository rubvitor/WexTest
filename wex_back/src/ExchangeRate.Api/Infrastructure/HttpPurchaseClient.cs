using System.Net.Http.Json;
using Wex.ExchangeRate.Api.Application;

namespace Wex.ExchangeRate.Api.Infrastructure;

public sealed class HttpPurchaseClient(HttpClient httpClient) : IPurchaseClient
{
    public async Task<PurchaseDto?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        var response = await httpClient.GetAsync($"/api/purchases/{id}", cancellationToken);
        return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<PurchaseDto>(cancellationToken: cancellationToken) : null;
    }
}
