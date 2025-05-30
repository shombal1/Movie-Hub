using MongoDB.Bson;
using MovieHub.Engine.Domain.DomainEvents;
using MovieHub.Engine.Domain.UseCases;
using MovieHub.Engine.Storage.Common;

namespace MovieHub.Engine.Storage.Storages;

public class DomainEventStorage(
    MovieHubDbContext dbContext,
    IGuidFactory guidFactory,
    TimeProvider timeProvider) : IDomainEventStorage
{
    public async Task AddEvent(DomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var entity = new Entities.DomainEvent
        {
            Id = guidFactory.Create(),
            EmittedAt = timeProvider.GetUtcNow(),
            Content = domainEvent.ToBsonDocument()
        };

        await dbContext.DomainEvents.InsertOneAsync(dbContext.CurrentSession, entity,
            cancellationToken: cancellationToken);
    }
}