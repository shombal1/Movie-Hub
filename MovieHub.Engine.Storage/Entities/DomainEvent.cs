
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MovieHub.Engine.Storage.Entities;

public class DomainEvent
{
    [BsonId]
    public Guid Id { get; set; }
    
    [BsonElement("emittedAt")]
    public DateTimeOffset EmittedAt { get; set; }
    
    [BsonElement("content")]
    public BsonDocument Content { get; set; } 
}