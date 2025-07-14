namespace MovieHub.Engine.Api.Models.Requests;

public class CreateMovieRequestDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateOnly ReleasedAt { get; set; }
    public IEnumerable<string> Countries { get; set; }
    public IEnumerable<string> Genres { get; set; }
    public IEnumerable<Guid> DirectorIds { get; set; }
    public IEnumerable<Guid> ActorsIds { get; set; }
    public string AgeRating { get; set; }
    public long? Budget { get; set; }
}