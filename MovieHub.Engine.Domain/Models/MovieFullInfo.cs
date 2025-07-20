using MovieHub.Engine.Domain.Enums;

namespace MovieHub.Engine.Domain.Models;

public class MovieFullInfo : MediaFullInfo
{
    // s3Key
    public Dictionary<QualityType, string> AvailableQualities { get; set; } = new();
}