using System.Text.Json.Serialization;

namespace MovieHub.Engine.Domain.Models;

public abstract class Media
{
    public Guid Id { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public DateOnly ReleasedAt { get; set; }
    public DateTimeOffset PublishedAt { get; set; }
    public string Country { get; set; } = "";
    public IEnumerable<string> Genre { get; set; } = null!;
    public IEnumerable<string> Director { get; set; } = null!;
}