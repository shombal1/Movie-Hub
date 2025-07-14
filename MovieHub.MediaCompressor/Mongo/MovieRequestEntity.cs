using MongoDB.Bson.Serialization.Attributes;

namespace MovieHub.MediaCompressor.Mongo;

public class MovieRequestEntity
{
    [BsonId]
    public Guid Id { get; set; }
    
    [BsonElement("title")]
    public string Title { get; set; } = "";
    
    [BsonElement("description")]
    public string Description { get; set; } = "";

    [BsonElement("releasedAt")]
    public DateOnly ReleasedAt { get; set; }

    [BsonElement("countries")]
    public IEnumerable<string> Countries { get; set; } = null!;

    [BsonElement("genres")]
    public IEnumerable<string> Genres { get; set; } = null!;

    [BsonElement("directors")]
    public IEnumerable<Guid> Directors { get; set; } = null!;
    
    // contain keys for s3 storage
    [BsonElement("originalUrlKey")] 
    [BsonIgnoreIfNull]
    public string? OriginalUrlKey { get; set; }
    
    [BsonElement("duration")]
    [BsonIgnoreIfNull]
    public TimeSpan? Duration { get; set; }
    
    [BsonElement("actors")]
    public IEnumerable<Guid> Actors { get; set; } = null!;

    [BsonElement("ageRating")]
    public string AgeRating { get; set; } = "";
    
    [BsonElement("budget")]
    [BsonIgnoreIfNull]
    public long? Budget { get; set; }
    
    [BsonElement("status")] 
    public ProcessingStatus Status { get; set; } = new ProcessingStatus();

}