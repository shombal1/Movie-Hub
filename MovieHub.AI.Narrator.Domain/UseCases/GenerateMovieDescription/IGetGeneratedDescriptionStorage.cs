using MovieHub.AI.Narrator.Domain.Models;

namespace MovieHub.AI.Narrator.Domain.UseCases.GenerateMovieDescription;

public interface IGetGeneratedDescriptionStorage
{
    public Task<string?> Get(Guid movieId,CancellationToken cancellationToken);
}

public static class GetGeneratedDescriptionStorageExtensions
{
    public static async Task<bool> Exists(this IGetGeneratedDescriptionStorage storage, Guid movieId, CancellationToken cancellationToken)
    {
        var description = await storage.Get(movieId, cancellationToken);
        return description is not null;
    }
}