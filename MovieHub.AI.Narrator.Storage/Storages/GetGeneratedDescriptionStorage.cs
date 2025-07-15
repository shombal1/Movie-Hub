using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MovieHub.AI.Narrator.Domain.UseCases.GenerateMovieDescription;

namespace MovieHub.AI.Narrator.Storage.Storages;

public class GetGeneratedDescriptionStorage(MovieHubDbContext dbContext) : IGetGeneratedDescriptionStorage
{
    public async Task<string?> Get(Guid movieId, CancellationToken cancellationToken)
    {
        return await dbContext.MovieRequests
            .AsQueryable()
            .Where(m => m.Id == movieId)
            .Select(m=>m.Status.AiDescription)
            .FirstOrDefaultAsync(cancellationToken);
    }
}