using MongoDB.Bson;
using MongoDB.Driver;
using MovieHub.Engine.Domain.Models;
using MovieHub.Engine.Domain.UseCases.GetMediaFullInfo;
using MovieHub.Engine.Storage.Entities;

namespace MovieHub.Engine.Storage.Storages;

public class GetMediaFullInfoStorage(MovieHubDbContext dbContext) : IGetMediaFullInfoStorage
{
    public async Task<MediaFullInfo?> Get(Guid id, CancellationToken cancellationToken)
    {
        var addFieldsStage = new BsonDocument
        {
            {
                "$addFields", new BsonDocument
                {
                    { "mainInfo", new BsonDocument { { "$arrayElemAt", new BsonArray { "$mainInfoList", 0 } } } }
                }
            }
        };
        
        AggregateFacetResults? result = await dbContext.AdditionMediaInfo.Aggregate(dbContext.CurrentSession)
            .Match(m => m.MediaId == id)
            .Lookup(
                foreignCollection: dbContext.Media,
                localField: m => m.MediaId,
                foreignField: m => m.Id,
                @as: (AdditionMediaInfoEntity m) => m.MainInfoList
            )
            .AppendStage<AdditionMediaInfoEntity>(addFieldsStage)
            .Facet(
                AggregateFacet.Create("Series",
                    PipelineDefinition<AdditionMediaInfoEntity, SeriesFullInfo>.Create(
                    [
                        PipelineStageDefinitionBuilder.OfType<AdditionMediaInfoEntity, AdditionSeriesInfoEntity>(),
                        PipelineStageDefinitionBuilder
                            .Lookup<AdditionSeriesInfoEntity, SeasonEntity, AdditionSeriesInfoEntity>(
                                foreignCollection: dbContext.Seasons,
                                localField: m => m.MediaId,
                                foreignField: s => s.SeriesId,
                                @as: m => m.Seasons
                            ),
                        PipelineStageDefinitionBuilder.Project<AdditionSeriesInfoEntity, SeriesFullInfo>(
                            x => new SeriesFullInfo()
                            {
                                Actors = x.Actors,
                                Countries = x.MainInfo.Countries,
                                Directors = x.MainInfo.Directors,
                                Genres = x.MainInfo.Genres,
                                CountEpisodes = (x.MainInfo as SeriesEntity)!.CountEpisodes,
                                CountSeasons = (x.MainInfo as SeriesEntity)!.CountSeasons,
                                Description = x.MainInfo.Description,
                                PublishedAt = x.MainInfo.PublishedAt,
                                ReleasedAt = x.MainInfo.ReleasedAt,
                                Title = x.MainInfo.Title,
                                Views = x.MainInfo.Views,
                                Budget = x.Budget,
                                AgeRating = x.AgeRating,
                                Seasons = x.Seasons.Select(s => new Season()
                                {
                                    Id = s.Id,
                                    Number = s.Number,
                                    ReleaseYearAt = s.ReleaseYearAt,
                                    Episodes = s.Episodes.Select(e => new Episode()
                                    {
                                        EpisodeNumber = e.Number,
                                        StreamingUrl = e.StreamingUrl,
                                        Title = e.Title,
                                    }),
                                })
                            }
                        )
                    ])
                ),
                AggregateFacet.Create("Movie",
                    PipelineDefinition<AdditionMediaInfoEntity, MovieFullInfo>.Create([
                        PipelineStageDefinitionBuilder.OfType<AdditionMediaInfoEntity, AdditionMovieInfoEntity>(),
                        PipelineStageDefinitionBuilder.Project<AdditionMovieInfoEntity, MovieFullInfo>(
                            x => new MovieFullInfo()
                            {
                                Actors = x.Actors,
                                Countries = x.MainInfo.Countries,
                                Directors = x.MainInfo.Directors,
                                Genres = x.MainInfo.Genres,
                                Quality = (x.MainInfo as MovieEntity)!.Quality,
                                Description = x.MainInfo.Description,
                                PublishedAt = x.MainInfo.PublishedAt,
                                ReleasedAt = x.MainInfo.ReleasedAt,
                                StreamingUrl = x.StreamingUrl,
                                Title = x.MainInfo.Title,
                                Views = x.MainInfo.Views,
                                Budget = x.Budget,
                                AgeRating = x.AgeRating,
                                Duration = x.Duration
                            }
                        )
                    ])
                )
            )
            .FirstOrDefaultAsync(cancellationToken);

        if (result is null)
            return null;

        var series = result.Facets.First(x => x.Name == "Series").Output<SeriesFullInfo>().FirstOrDefault();
        var movies = result.Facets.First(x => x.Name == "Movie").Output<MovieFullInfo>().FirstOrDefault();

        if (series is null && movies is null)
            return null;

        if (series is null)
        {
            movies.Id = id;
            return movies;
        }

        if (movies is null)
        {
            series.Id = id;
            return series;
        }

        return null;
    }
}