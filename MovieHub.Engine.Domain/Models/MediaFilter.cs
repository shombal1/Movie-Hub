using MovieHub.Engine.Domain.UseCases.GetMedia;

namespace MovieHub.Engine.Domain.Models;

public record MediaFilter()
{
    public ParameterSorting ParameterSorting { get; set; } = ParameterSorting.Alphabetically;
    public TypeSorting TypeSorting { get; set; } = TypeSorting.Ascending;
    public IEnumerable<string> Countries { get; set; } = [];
    public bool MatchAllCountries { get; set; } = false;
    public IEnumerable<string> Genres { get; set; } = [];
    public bool MatchAllGenres { get; set; } = false;
    public IEnumerable<int> Years { get; set; } = [];
    public int Skip { get; set; } = 0;
    public int Take { get; set; } = 10;

    public MediaFilter(
        ParameterSorting parameterSorting, TypeSorting typeSorting,
        IEnumerable<string> countries, bool matchAllCountries,
        IEnumerable<string> genres, bool matchAllGenres,
        IEnumerable<int> years,
        int skip, int take) : this()
    {
        ParameterSorting = parameterSorting;
        TypeSorting = typeSorting;
        Countries = countries;
        MatchAllCountries = matchAllCountries;
        Genres = genres;
        MatchAllGenres = matchAllGenres;
        Years = years;
        Skip = skip;
        Take = take;
    }
}