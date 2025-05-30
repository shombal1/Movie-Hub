using MongoDB.Driver;
using MovieHub.Engine.Domain.UseCases.AddMedia.FinalizeMovieAddition;
using MovieHub.Engine.Storage.Entities;


namespace MovieHub.Engine.Storage.Storages.AddMovie;

public class FinalizeMovieAdditionStorage(MovieHubDbContext dbContext) : IFinalizeMovieAdditionStorage
{
    public  Task Update(Guid requestId, CancellationToken cancellation)
    {
        var filter = Builders<MovieRequestEntity>.Filter.Where(x => x.Id == requestId);
        var update = Builders<MovieRequestEntity>.Update
            .Set(x => x.Status.IsFinalizeMovieAddition , true);
        
        return dbContext.MovieRequests
            .FindOneAndUpdateAsync(dbContext.CurrentSession,filter, update, cancellationToken: cancellation);
    }

}