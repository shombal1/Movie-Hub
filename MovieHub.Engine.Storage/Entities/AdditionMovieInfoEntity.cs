using MongoDB.Bson.Serialization.Attributes;

namespace MovieHub.Engine.Storage.Entities;

[BsonDiscriminator("additionMovieInfo")]
public class AdditionMovieInfoEntity : AdditionMediaInfoEntity
{
    [BsonElement("streamingUrl")] 
    public string StreamingUrl { get; set; } = "";
    
    [BsonElement("duration")]
    public TimeSpan Duration { get; set; }
}