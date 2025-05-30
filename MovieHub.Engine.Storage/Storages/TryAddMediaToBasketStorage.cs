using MongoDB.Driver;
using MovieHub.Engine.Domain.UseCases.AddMediaToBasket;
using MovieHub.Engine.Storage.Common;
using MovieHub.Engine.Storage.Entities;

namespace MovieHub.Engine.Storage.Storages;

public class TryAddMediaToBasketStorage(
    MovieHubDbContext dbContext, 
    IGuidFactory guidFactory,
    TimeProvider timeProvider)
    : ITryAddMediaToBasketStorage
{
    public async Task<bool> TryAdd(Guid userId, Guid mediaId, CancellationToken cancellationToken)
    {
        try
        {
            await dbContext.MediaBasket.InsertOneAsync(
                dbContext.CurrentSession,
                new MediaBasketEntity()
                {
                    Id = guidFactory.Create(),
                    MediaId = mediaId,
                    UserId = userId,
                    AddedAt = timeProvider.GetUtcNow()
                }, null, cancellationToken);
        }
        catch (MongoWriteException e) when (e.WriteError.Category == ServerErrorCategory.DuplicateKey)
        {
            return false;
        }

        return true;
    }
}