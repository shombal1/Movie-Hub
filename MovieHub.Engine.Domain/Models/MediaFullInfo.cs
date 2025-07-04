namespace MovieHub.Engine.Domain.Models;

public abstract class MediaFullInfo
{
    public Guid Id { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public DateOnly ReleasedAt { get; set; }
    public DateTimeOffset PublishedAt { get; set; }
    public long Views { get; set; }
    public IEnumerable<string> Countries { get; set; } = null!;
    public IEnumerable<string> Genres { get; set; } = null!;
    public IEnumerable<BasePersonInfo> Directors { get; set; } = null!;
    public IEnumerable<BasePersonInfo> Actors { get; set; } = null!;
    public string AgeRating { get; set; } = "";
    public long? Budget { get; set; }
}