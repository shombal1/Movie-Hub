using MovieHub.Engine.Domain.Models;

namespace MovieHub.Engine.Domain.UseCases.GetMediaFullInfo;

public interface IGetMediaFullInfoStorage : IStorage
{
    public Task<MediaFullInfo?> Get(Guid id, CancellationToken cancellationToken);
}