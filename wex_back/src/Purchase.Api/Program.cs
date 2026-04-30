using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wex.Purchase.Api;
using Wex.Purchase.Api.Application;
using Wex.Purchase.Api.Infrastructure.Persistence;
using Wex.SharedKernel;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IEventBus, InMemoryEventBus>();
builder.Services.AddDbContext<PurchaseDbContext>(options => options.UseInMemoryDatabase("WexPurchaseDatabase"));
builder.Services.AddScoped<IPurchaseRepository, EfPurchaseRepository>();
builder.Services.AddScoped<PurchaseService>();
builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();
app.UseCors();
app.UseSwagger();
app.UseSwaggerUI();

app.MapPost("/api/purchases", async ([FromBody] StorePurchaseRequest request, PurchaseService service, CancellationToken ct) =>
{
    var result = await service.StoreAsync(request, ct);
    return result.IsSuccess ? Results.Created($"/api/purchases/{result.Value!.Id}", result.Value) : Results.BadRequest(result.Error);
});

app.MapGet("/api/purchases/{id:guid}", async (Guid id, PurchaseService service, CancellationToken ct) =>
    await service.GetAsync(id, ct) is { } purchase ? Results.Ok(purchase) : Results.NotFound());

app.MapGet("/api/purchases", async (PurchaseService service, CancellationToken ct) => Results.Ok(await service.ListAsync(ct)));

app.Run();

public partial class Program { }
