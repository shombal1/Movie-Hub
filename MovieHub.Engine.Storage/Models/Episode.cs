using MongoDB.Bson.Serialization.Attributes;

namespace MovieHub.Engine.Storage.Models;

public class Episode
{
    [BsonElement("number")]
    public int Number { get; set; }

    [BsonElement("title")] 
    public string Title { get; set; } = "";
    
    [BsonElement("streamingUrl")]
    public string StreamingUrl { get; set; } = "";
}