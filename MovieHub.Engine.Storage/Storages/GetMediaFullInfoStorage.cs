using AutoMapper;
using MongoDB.Bson;
using MongoDB.Driver;
using MovieHub.Engine.Domain.Models;
using MovieHub.Engine.Domain.UseCases.GetMediaFullInfo;
using MovieHub.Engine.Storage.Common;
using MovieHub.Engine.Storage.Entities;

namespace MovieHub.Engine.Storage.Storages;

public class GetMediaFullInfoStorage(MovieHubDbContext dbContext, IMapper mapper) : IGetMediaFullInfoStorage
{
    public async Task<MediaFullInfo?> Get(Guid id, CancellationToken cancellationToken)
    {
        var facetResults = await dbContext.AdditionMediaInfo
            .Aggregate(dbContext.CurrentSession)
            .Match(m => m.MediaId == id)
            .Lookup(
                foreignCollection: dbContext.Media,
                localField: m => m.MediaId,
                foreignField: m => m.Id,
                @as: (AdditionMediaInfoEntity m) => m.MainInfoList
            )
            .Facet(
                AggregateFacet.Create("series",
                    new EmptyPipelineDefinition<AdditionMediaInfoEntity>()
                        .Match(Builders<AdditionMediaInfoEntity>.Filter.Eq("_t", "additionSeriesInfo"))
                        .Lookup(
                            foreignCollection: dbContext.Seasons,
                            localField: m => m.MediaId,
                            foreignField: s => s.SeriesId,
                            @as: (AdditionSeriesInfoEntity m) => m.Seasons
                        )
                ),
                AggregateFacet.Create("movie",
                    new EmptyPipelineDefinition<AdditionMediaInfoEntity>()
                        .Match(Builders<AdditionMediaInfoEntity>.Filter.Eq("_t", "additionMovieInfo"))
                )
            )
            .FirstOrDefaultAsync(cancellationToken);

        if (facetResults == null)
            return null;

        var seriesFacet = facetResults.Facets.FirstOrDefault(x => x.Name == "series");
        var movieFacet = facetResults.Facets.FirstOrDefault(x => x.Name == "movie");

        AdditionMediaInfoEntity? result = null;
    
        if (seriesFacet?.Output<AdditionSeriesInfoEntity>().Any() == true)
        {
            result = seriesFacet.Output<AdditionSeriesInfoEntity>().First();
        }
        else if (movieFacet?.Output<AdditionMediaInfoEntity>().Any() == true)
        {
            result = movieFacet.Output<AdditionMediaInfoEntity>().First();
        }

        if (result == null)
            return null;

        result.MainInfo = result.MainInfoList.First();

        return mapper.Map<MediaFullInfo>(result);
    }
}