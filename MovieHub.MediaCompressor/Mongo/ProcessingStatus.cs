using MongoDB.Bson.Serialization.Attributes;

namespace MovieHub.MediaCompressor.Mongo;

public class ProcessingStatus
{
    [BsonElement("isFinalizeMovieAddition")]
    public bool IsFinalizeMovieAddition { get; set; }
    
    [BsonElement("processedQualities")]
    public Dictionary<QualityType, string> ProcessedQualities { get; set; } = new();
    
    [BsonElement("isQualitiesProcessed")]
    public bool IsQualitiesProcessed { get; set; }
    
    [BsonElement("aiDescription")]
    public string? AiDescription { get; set; }
    
    [BsonElement("isFullyProcessed")]
    public bool IsFullyProcessed { get; set; }

    [BsonElement("processingErrors")]
    [BsonIgnoreIfNull]
    public List<string> ProcessingErrors { get; set; } = [];
}