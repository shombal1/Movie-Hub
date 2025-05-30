using AutoMapper;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MovieHub.Engine.Domain.Models;
using MovieHub.Engine.Domain.UseCases.GetMediaFromBasket;

namespace MovieHub.Engine.Storage.Storages;

public class GetMediaFromBasketStorage(MovieHubDbContext dbContext, IMapper mapper) : IGetMediaFromBasketStorage
{
    public async Task<IEnumerable<Media>> Get(Guid userId, int skip, int take, CancellationToken cancellationToken)
    {
        var media = await dbContext.MediaBasket
            .AsQueryable(dbContext.CurrentSession)
            .Where(m => m.UserId == userId)
            .OrderByDescending(m=>m.AddedAt)
            .Join(dbContext.Media.AsQueryable(),
                m => m.MediaId,
                m => m.Id,
                (b, m) => m)
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken: cancellationToken);

        return media.Select(mapper.Map<Media>);
    }
}