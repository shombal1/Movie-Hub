using MovieHub.Engine.Domain.Models;

namespace MovieHub.Engine.Domain.UseCases.AddMedia.PublishMovieRequest;

public interface ICreateMovieStorage : IStorage
{
    public Task Create(MovieRequest request ,CancellationToken cancellationToken);
}