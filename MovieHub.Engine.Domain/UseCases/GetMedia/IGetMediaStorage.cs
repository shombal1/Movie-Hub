using MovieHub.Engine.Domain.Models;

namespace MovieHub.Engine.Domain.UseCases.GetMedia;

public interface IGetMediaStorage : IStorage
{
    public Task<IEnumerable<Media>> Get(
        MediaFilter mediaFilter, 
        CancellationToken cancellationToken);

    public Task<Media?> Get(Guid id, CancellationToken cancellationToken);
}