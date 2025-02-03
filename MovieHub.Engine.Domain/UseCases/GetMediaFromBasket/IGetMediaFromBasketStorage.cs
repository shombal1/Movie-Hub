using MovieHub.Engine.Domain.Models;

namespace MovieHub.Engine.Domain.UseCases.GetMediaFromBasket;

public interface IGetMediaFromBasketStorage : IStorage
{
    public Task<IEnumerable<Media>> Get(Guid userId, int skip, int take,CancellationToken cancellationToken);
}