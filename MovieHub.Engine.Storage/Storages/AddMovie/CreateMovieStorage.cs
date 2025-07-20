using AutoMapper;
using AutoMapper.QueryableExtensions;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MovieHub.Engine.Domain.Models;
using MovieHub.Engine.Domain.UseCases.AddMedia.PublishMovieRequest;
using MovieHub.Engine.Storage.Entities;
using BasePersonInfo = MovieHub.Engine.Storage.Models.BasePersonInfo;

namespace MovieHub.Engine.Storage.Storages.AddMovie;

public class CreateMovieStorage(
    MovieHubDbContext dbContext,
    TimeProvider timeProvider,
    IMapper mapper)
    : ICreateMovieStorage
{
    public async Task Create(MovieRequest request, CancellationToken cancellationToken)
    {
        var session = dbContext.CurrentSession;
        var transactionOwned = !session.IsInTransaction;

        if (transactionOwned)
        {
            session.StartTransaction();
        }

        try
        {
            var movieEntity = new MovieEntity()
            {
                Countries = request.Countries,
                Description = request.Description,
                Genres = request.Genres,
                Id = request.Id,
                PublishedAt = timeProvider.GetUtcNow(),
                ReleasedAt = request.ReleasedAt,
                ReleasedYearAt = request.ReleasedAt.Year,
                Title = request.Title,
                Views = 0
            };

            var addMovieTask = dbContext.Media.InsertOneAsync(
                session,
                movieEntity,
                cancellationToken: cancellationToken);

            var allPersonIds = request.ActorIds.Concat(request.DirectorIds).Distinct();
            var allPersons = await dbContext.Persons
                .AsQueryable(session)
                .Where(x => allPersonIds.Contains(x.Id))
                .ProjectTo<BasePersonInfo>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            var actors = allPersons.Where(p => request.ActorIds.Contains(p.Id)).ToList();
            var directors = allPersons.Where(p => request.DirectorIds.Contains(p.Id)).ToList();

            var addAdditionMovieInfoTask = dbContext.AdditionMediaInfo.InsertOneAsync(
                session,
                new AdditionMovieInfoEntity()
                {
                    MediaId = request.Id,
                    Actors = actors,
                    Directors = directors,
                    AgeRating = request.AgeRating,
                    Budget = request.Budget,
                    AiDescription = request.Status.AiDescription!,
                    AvailableQualities = request.Status.ProcessedQualities
                        .ToDictionary(
                            kvp => mapper.Map<Common.QualityType>(kvp.Key),
                            kvp => kvp.Value)
                },
                cancellationToken: cancellationToken);

            await Task.WhenAll(addMovieTask, addAdditionMovieInfoTask);

            if (transactionOwned)
            {
                await session.CommitTransactionAsync(cancellationToken);
            }
        }
        catch
        {
            if (transactionOwned && session.IsInTransaction)
            {
                await session.AbortTransactionAsync(cancellationToken);
            }

            throw;
        }
    }
}