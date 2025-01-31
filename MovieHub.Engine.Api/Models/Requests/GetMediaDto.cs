using MovieHub.Engine.Api.Enums;

namespace MovieHub.Engine.Api.Models.Requests;

public class GetMediaDto
{
    public ParameterSorting ParameterSorting { get; set; }
    public TypeSorting TypeSorting { get; set; }
    public IEnumerable<string> Countries { get; set; } = [];
    public bool MatchAllCountries { get; set; }
    public IEnumerable<string> Genres { get; set; } = [];
    public bool MatchAllGenres { get; set; }
    public IEnumerable<int> Years { get; set; } = [];
}