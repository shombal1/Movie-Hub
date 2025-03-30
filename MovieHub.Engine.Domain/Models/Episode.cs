namespace MovieHub.Engine.Domain.Models;

public class Episode
{
    public int EpisodeNumber { get; set; }
    
    public string Title { get; set; } = "";
    
    public string StreamingUrl { get; set; } = "";
}