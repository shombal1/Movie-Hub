namespace MovieHub.Engine.Domain.UseCases.RemoveMediaFromBasket;

public interface IRemoveMediaFromBasketStorage : IStorage
{
    public Task<bool> Remove(Guid userId, Guid mediaId,CancellationToken cancellationToken);
}