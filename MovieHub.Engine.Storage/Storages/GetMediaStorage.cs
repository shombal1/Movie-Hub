using MapsterMapper;
using MongoDB.Driver;
using MovieHub.Engine.Domain.Models;
using MovieHub.Engine.Domain.UseCases.GetMedia;
using MovieHub.Engine.Storage.Entities;

namespace MovieHub.Engine.Storage.Storages;

public class GetMediaStorage(MovieHubDbContext dbContext,IMapper mapper) : IGetMediaStorage
{
    public async Task<IEnumerable<Media>> Get(ParameterSorting parameterSorting, TypeSorting typeSorting,
        string? country, int? year, int skip, int take, CancellationToken cancellationToken)
    {
        SortDefinition<MediaEntity> sortDefinition = GetSortDefinition(parameterSorting, typeSorting);
        FilterDefinition<MediaEntity> filter = GetFilterDefinition(country, year);

        var medias = await dbContext.Media
            .Find(filter)
            .Sort(sortDefinition)
            .Skip(skip)
            .Limit(take)
            .ToListAsync(cancellationToken);

        return mapper.Map<IEnumerable<Media>>(medias);
    }

    private SortDefinition<MediaEntity> GetSortDefinition(ParameterSorting parameterSorting, TypeSorting typeSorting)
    {
        FieldDefinition<MediaEntity, object> fieldName = parameterSorting switch
        {
            ParameterSorting.Alphabetically => new ExpressionFieldDefinition<MediaEntity, object>(
                m => m.Title),
            ParameterSorting.ReleaseDate => new ExpressionFieldDefinition<MediaEntity, object>(m => 
                m.ReleasedAt),
            ParameterSorting.PublicationDate => new ExpressionFieldDefinition<MediaEntity, object>(m=>
                m.PublishedAt),
            _ => throw new ArgumentOutOfRangeException(nameof(parameterSorting), parameterSorting, null)
        };

        SortDefinition<MediaEntity> sortDefinition = typeSorting switch
        {
            TypeSorting.Ascending => Builders<MediaEntity>.Sort.Ascending(fieldName),
            TypeSorting.Descending => Builders<MediaEntity>.Sort.Descending(fieldName),
            _ => throw new ArgumentOutOfRangeException(nameof(typeSorting), typeSorting, null)
        };

        return sortDefinition;
    }

    private FilterDefinition<MediaEntity> GetFilterDefinition(string? country, int? year)
    {
        List<FilterDefinition<MediaEntity>> filters = new List<FilterDefinition<MediaEntity>>(2);

        if (country != null)
            filters.Add(new ExpressionFilterDefinition<MediaEntity>(m => m.Country == country));

        if (year != null)
            filters.Add(new ExpressionFilterDefinition<MediaEntity>(m =>
                    m.ReleasedAt >= new DateOnly(year.Value, 1, 1) && m.ReleasedAt<=new DateOnly(year.Value,12,31)));

        FilterDefinition<MediaEntity> filter = FilterDefinition<MediaEntity>.Empty;

        if (filters.Count > 0)
            filter = new FilterDefinitionBuilder<MediaEntity>().And(filters);

        return filter;
    }
}