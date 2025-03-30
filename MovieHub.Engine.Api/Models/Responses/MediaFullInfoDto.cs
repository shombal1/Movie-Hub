using System.Text.Json.Serialization;

namespace MovieHub.Engine.Api.Models.Responses;

[JsonDerivedType(typeof(MovieFullInfoDto), "movie")] 
[JsonDerivedType(typeof(SeriesFullInfoDto),"series")]
public class MediaFullInfoDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public DateOnly ReleasedAt { get; set; }
    public DateTimeOffset PublishedAt { get; set; }
    public long Views { get; set; }
    public IEnumerable<string> Countries { get; set; } = null!;
    public IEnumerable<string> Genres { get; set; } = null!;
    public IEnumerable<string> Directors { get; set; } = null!;
    public IEnumerable<string> Actors { get; set; } = null!;
    public string AgeRating { get; set; } = "";
    public long? Budget { get; set; }
}