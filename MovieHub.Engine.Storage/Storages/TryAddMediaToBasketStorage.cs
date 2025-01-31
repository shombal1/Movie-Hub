using MapsterMapper;
using MongoDB.Driver;
using MovieHub.Engine.Domain.UseCases.TryAddMovieToBasket;
using MovieHub.Engine.Storage.Entities;

namespace MovieHub.Engine.Storage.Storages;

public class TryAddMediaToBasketStorage(MovieHubDbContext dbContext, IMapper mapper,IGuidFactory guidFactory) : ITryAddMediaToBasketStorage
{
    public async Task<bool> TryAdd(Guid userId, Guid mediaId, CancellationToken cancellationToken)
    {
        try
        {        
            await dbContext.MovieBaskets.InsertOneAsync(new MediaBasketEntity()
            {
                Id = guidFactory.Create(),
                MediaId = mediaId,
                UserId = userId,
            }, null, cancellationToken);
        }
        catch (MongoWriteException e) when (e.WriteError.Category == ServerErrorCategory.DuplicateKey)
        {
            return false;
        }

        return true;
    }
}