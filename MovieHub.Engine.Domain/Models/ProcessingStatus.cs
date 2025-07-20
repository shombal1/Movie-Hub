using MovieHub.Engine.Domain.Enums;

namespace MovieHub.Engine.Domain.Models;

public class ProcessingStatus
{
    public bool IsFinalizeMovieAddition { get; set; }
    
    public Dictionary<QualityType, string> ProcessedQualities { get; set; } = new();
    
    public string? AiDescription { get; set; }
    public bool IsQualitiesProcessed { get; set; }
    
    public bool IsFullyProcessed { get; set; }
    
    public List<string> ProcessingErrors { get; set; } = [];
}