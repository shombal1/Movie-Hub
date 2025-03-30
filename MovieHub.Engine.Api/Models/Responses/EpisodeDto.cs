namespace MovieHub.Engine.Api.Models.Responses;

public class EpisodeDto
{
    public int EpisodeNumber { get; set; }
    
    public string Title { get; set; } = "";
    
    public string StreamingUrl { get; set; } = "";
}