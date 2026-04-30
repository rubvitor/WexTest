namespace Wex.SharedKernel;

public interface IEventBus
{
    Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken) where TEvent : IDomainEvent;
}

public sealed class InMemoryEventBus : IEventBus
{
    public List<IDomainEvent> PublishedEvents { get; } = [];

    public Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken) where TEvent : IDomainEvent
    {
        PublishedEvents.Add(@event);
        return Task.CompletedTask;
    }
}
