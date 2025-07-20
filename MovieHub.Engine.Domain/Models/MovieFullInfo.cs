using MovieHub.Engine.Domain.Enums;

namespace MovieHub.Engine.Domain.Models;

public class MovieFullInfo : MediaFullInfo
{
    public string AiDescription { get; set; } = "";
    // s3Key
    public Dictionary<QualityType, string> AvailableQualities { get; set; } = new();
}