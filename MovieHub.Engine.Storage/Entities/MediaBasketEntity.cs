using MongoDB.Bson.Serialization.Attributes;

namespace MovieHub.Engine.Storage.Entities;

public class MediaBasketEntity
{
    [BsonId]
    public Guid Id { get; set; }
    
    [BsonElement("userId")]
    public Guid UserId { get; set; }
    
    [BsonElement("mediaId")]
    public Guid MediaId { get; set; }
    
    public DateTimeOffset AddedAt { get; set; }
}