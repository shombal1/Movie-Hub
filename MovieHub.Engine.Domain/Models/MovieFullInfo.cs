namespace MovieHub.Engine.Domain.Models;

public class MovieFullInfo : MediaFullInfo
{
    public string Quality { get; set; } = "";
    public string StreamingUrl { get; set; } = "";
    public TimeSpan Duration { get; set; } 
}