namespace MovieHub.Engine.Domain.UseCases.TryAddMovieToBasket;

public interface ITryAddMediaToBasketStorage : IStorage
{
    public Task<bool> TryAdd(Guid userId, Guid mediaId, CancellationToken cancellationToken);
}