using System.Text.Json.Serialization;
using MovieHub.Engine.Api.Enums;

namespace MovieHub.Engine.Api.Models.Responses;

[JsonDerivedType(typeof(MovieFullInfoDto), "movie")] 
public class MovieFullInfoDto : MediaFullInfoDto
{
    public Dictionary<QualityType, string> AvailableQualities { get; set; } = new();
}