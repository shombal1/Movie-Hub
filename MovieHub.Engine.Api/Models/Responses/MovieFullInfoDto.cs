using System.Text.Json.Serialization;

namespace MovieHub.Engine.Api.Models.Responses;

[JsonDerivedType(typeof(MovieFullInfoDto), "movie")] 
public class MovieFullInfoDto : MediaFullInfoDto
{
    public string Quality { get; set; } = "";
    public string StreamingUrl { get; set; } = "";
    public TimeSpan Duration { get; set; } 
}