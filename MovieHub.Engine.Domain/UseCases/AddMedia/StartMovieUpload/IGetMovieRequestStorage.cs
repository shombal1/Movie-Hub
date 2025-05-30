using MovieHub.Engine.Domain.Models;

namespace MovieHub.Engine.Domain.UseCases.AddMedia.StartMovieUpload;

public interface IGetMovieRequestStorage : IStorage
{
    public Task<MovieRequest?> GetMovieRequest(Guid id,CancellationToken cancellation);
    public Task<bool> Exists(Guid requestId, CancellationToken cancellationToken);
}