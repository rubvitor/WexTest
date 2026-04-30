using Wex.Purchase.Api.Domain;
using Wex.SharedKernel;

namespace Wex.Purchase.Api.Application;

public interface IPurchaseRepository
{
    Task SaveAsync(PurchaseTransaction purchase, CancellationToken cancellationToken);
    Task<PurchaseTransaction?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<PurchaseTransaction>> ListAsync(CancellationToken cancellationToken);
}

public sealed class PurchaseService(IPurchaseRepository repository, IEventBus eventBus)
{
    public async Task<Result<PurchaseResponse>> StoreAsync(StorePurchaseRequest request, CancellationToken cancellationToken)
    {
        var created = PurchaseTransaction.Create(request.Description, request.TransactionDate, request.PurchaseAmountUsd);
        if (!created.IsSuccess) return Result<PurchaseResponse>.Failure(created.Error!.Code, created.Error.Message);

        var purchase = created.Value!;
        await repository.SaveAsync(purchase, cancellationToken);
        await eventBus.PublishAsync(new PurchaseStoredEvent(Guid.NewGuid(), DateTimeOffset.UtcNow, purchase.Id, purchase.Description, purchase.TransactionDate, purchase.Amount.Amount), cancellationToken);

        return Result<PurchaseResponse>.Success(purchase.ToResponse());
    }

    public async Task<PurchaseResponse?> GetAsync(Guid id, CancellationToken cancellationToken)
        => (await repository.GetByIdAsync(id, cancellationToken))?.ToResponse();

    public async Task<IReadOnlyCollection<PurchaseResponse>> ListAsync(CancellationToken cancellationToken)
        => (await repository.ListAsync(cancellationToken)).Select(x => x.ToResponse()).ToArray();
}

public static class PurchaseMappings
{
    public static PurchaseResponse ToResponse(this PurchaseTransaction purchase)
        => new(purchase.Id, purchase.Description, purchase.TransactionDate, purchase.Amount.Amount);
}
