using AutoMapper;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MovieHub.Engine.Domain.Models;
using MovieHub.Engine.Domain.UseCases.AddMedia.FinalizeMovieAddition;
using MovieHub.Engine.Domain.UseCases.AddMedia.StartMovieUpload;

namespace MovieHub.Engine.Storage.Storages.AddMovie;

public class GetMovieRequestStorage(MovieHubDbContext dbContext, IMapper mapper) : IGetMovieRequestStorage
{
    public async Task<MovieRequest?> GetMovieRequest(Guid id, CancellationToken cancellation)
    {
        var entity = await dbContext.MovieRequests
            .AsQueryable(dbContext.CurrentSession)
            .FirstOrDefaultAsync(x => x.Id == id, cancellation);

        return entity is null ? null : mapper.Map<MovieRequest>(entity);
    }

    public Task<bool> Exists(Guid requestId, CancellationToken cancellationToken)
    {
        return dbContext.MovieRequests
            .AsQueryable(dbContext.CurrentSession)
            .AnyAsync(x => x.Id == requestId, cancellationToken);
    }
}