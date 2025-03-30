namespace MovieHub.Engine.Api.Models.Responses;

public class SeasonDto
{
    public Guid Id { get; set; }
    
    public int Number { get; set; }
    
    public int ReleaseYearAt { get; set; }
    
    public IEnumerable<EpisodeDto> Episodes { get; set; } = null!;
}