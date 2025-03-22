using AutoMapper;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MovieHub.Engine.Domain.Models;
using MovieHub.Engine.Domain.UseCases.GetMedia;
using MovieHub.Engine.Storage.Entities;

namespace MovieHub.Engine.Storage.Storages;

public class GetMediaStorage(MovieHubDbContext dbContext, IMapper mapper, ILogger<GetMediaStorage> logger)
    : IGetMediaStorage
{
    public async Task<IEnumerable<Media>> Get(
        MediaFilter mediaFilter,
        CancellationToken cancellationToken)
    {
        SortDefinition<MediaEntity> sortDefinition =
            GetSortDefinition(mediaFilter.ParameterSorting, mediaFilter.TypeSorting);

        FilterDefinition<MediaEntity> filter = GetFilterDefinition(mediaFilter.Countries, mediaFilter.MatchAllCountries,
            mediaFilter.Genres, mediaFilter.MatchAllGenres, mediaFilter.Years);

        var medias = await dbContext.Media
            .Find(dbContext.CurrentSession,filter)
            .Sort(sortDefinition)
            .Skip(mediaFilter.Skip)
            .Limit(mediaFilter.Take)
            .ToListAsync(cancellationToken);

        return mapper.Map<IEnumerable<Media>>(medias);
    }

    public async Task<Media?> Get(Guid id, CancellationToken cancellationToken)
    { 
        return mapper.Map<IEnumerable<Media>>(await dbContext.Media.Find(
                dbContext.CurrentSession,Builders<MediaEntity>.Filter.Where(m => m.Id == id))
            .ToListAsync(cancellationToken)).FirstOrDefault();
    }

    private SortDefinition<MediaEntity> GetSortDefinition(ParameterSorting parameterSorting, TypeSorting typeSorting)
    {
        FieldDefinition<MediaEntity, object> fieldName = parameterSorting switch
        {
            ParameterSorting.Alphabetically => new ExpressionFieldDefinition<MediaEntity, object>(
                m => m.Title),
            ParameterSorting.ReleaseDate => new ExpressionFieldDefinition<MediaEntity, object>(m =>
                m.ReleasedAt),
            ParameterSorting.PublicationDate => new ExpressionFieldDefinition<MediaEntity, object>(m =>
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

    private FilterDefinition<MediaEntity> GetFilterDefinition(
        IEnumerable<string> countries,
        bool matchAllCountries,
        IEnumerable<string> genres,
        bool matchAllGenres,
        IEnumerable<int> years)
    {
        List<FilterDefinition<MediaEntity>> filters = new List<FilterDefinition<MediaEntity>>();

        if (countries.Any())
        {
            var countryFilter = matchAllCountries
                ? Builders<MediaEntity>.Filter.All(m => m.Countries, countries)
                : Builders<MediaEntity>.Filter.AnyIn(m => m.Countries, countries);

            filters.Add(countryFilter);
        }

        if (genres.Any())
        {
            var genreFilter = matchAllGenres
                ? Builders<MediaEntity>.Filter.All(m => m.Genres, genres)
                : Builders<MediaEntity>.Filter.AnyIn(m => m.Genres, genres);

            filters.Add(genreFilter);
        }

        if (years.Any())
        {
            var yearFilter = Builders<MediaEntity>.Filter.In(m => m.ReleasedYearAt, years);
            filters.Add(yearFilter);
        }

        return filters.Any()
            ? Builders<MediaEntity>.Filter.And(filters)
            : Builders<MediaEntity>.Filter.Empty;
    }
}