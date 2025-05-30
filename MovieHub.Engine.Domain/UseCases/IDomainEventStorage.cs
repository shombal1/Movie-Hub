using MovieHub.Engine.Domain.DomainEvents;

namespace MovieHub.Engine.Domain.UseCases;

public interface IDomainEventStorage : IStorage
{
    public Task AddEvent(DomainEvent domainEvent, CancellationToken cancellationToken);
}