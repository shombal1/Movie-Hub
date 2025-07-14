namespace MovieHub.Engine.Domain.Models;


public class MovieRequest
{
    public Guid Id { get; set; }

    public string Title { get; set; } = "";

    public string Description { get; set; } = "";

    public DateOnly ReleasedAt { get; set; }

    public IEnumerable<string> Countries { get; set; } = null!;

    public IEnumerable<string> Genres { get; set; } = null!;

    public IEnumerable<Guid> DirectorIds { get; set; } = null!;

    public string? OriginalUrlKey { get; set; }

    public TimeSpan? Duration { get; set; }

    public IEnumerable<Guid> ActorIds { get; set; } = null!;

    public string AgeRating { get; set; } = "";

    public long? Budget { get; set; }
    
    public ProcessingStatus Status { get; set; } = new ProcessingStatus();
}