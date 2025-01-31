using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MovieHub.Engine.Storage.Entities;

public class MediaBasketEntity
{
    [BsonId]
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid Id { get; set; }

    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    [BsonElement("userId")]
    public Guid UserId { get; set; }
    
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    [BsonElement("mediaId")]
    public Guid MediaId { get; set; }
}