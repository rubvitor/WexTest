using Microsoft.AspNetCore.Mvc;
using Wex.ExchangeRate.Api.Application;
using Wex.ExchangeRate.Api.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));
builder.Services.AddHttpClient<IExchangeRateProvider, TreasuryRateProvider>();
builder.Services.AddHttpClient<IPurchaseClient, HttpPurchaseClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["PurchaseApi:BaseUrl"] ?? "https://localhost:49831");
});
builder.Services.AddScoped<ExchangeRateService>();

var app = builder.Build();
app.UseCors();
app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/api/conversions/{purchaseId:guid}", async (Guid purchaseId, [FromQuery] string country, ExchangeRateService service, CancellationToken ct) =>
{
    var result = await service.ConvertAsync(purchaseId, country, ct);
    return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
});

app.Run();
public partial class Program { }
