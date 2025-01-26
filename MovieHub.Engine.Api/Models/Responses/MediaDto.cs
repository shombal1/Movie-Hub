using System.Text.Json.Serialization;

namespace MovieHub.Engine.Api.Models.Responses;

[JsonDerivedType(typeof(MovieDto))] 
[JsonDerivedType(typeof(SeriesDto))]
public class MediaDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public DateOnly ReleasedAt { get; set; }
    public DateTimeOffset PublishedAt { get; set; }
    public IEnumerable<string> Countries { get; set; } = null!;
    public IEnumerable<string> Genre { get; set; } = null!;
    public IEnumerable<string> Director { get; set; } = null!;
    public virtual string Type { get; } = null!;
}