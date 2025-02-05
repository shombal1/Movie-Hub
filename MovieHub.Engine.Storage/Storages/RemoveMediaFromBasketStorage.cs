using MongoDB.Driver;
using MovieHub.Engine.Domain.UseCases.RemoveMediaFromBasket;
using MovieHub.Engine.Storage.Entities;

namespace MovieHub.Engine.Storage.Storages;

public class RemoveMediaFromBasketStorage(MovieHubDbContext dbContext) : IRemoveMediaFromBasketStorage
{
    public async Task<bool> Remove(Guid userId, Guid mediaId, CancellationToken cancellationToken)
    {
        var result = await dbContext.MediaBasket.DeleteOneAsync(
            dbContext.CurrentSession,
            Builders<MediaBasketEntity>.Filter
                .Where(m => m.MediaId == mediaId && m.UserId == userId),
            cancellationToken: cancellationToken);

        return result.DeletedCount == 1;
    }
}