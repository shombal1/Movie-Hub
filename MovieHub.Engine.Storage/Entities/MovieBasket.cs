using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MovieHub.Engine.Storage.Entities;

public class MovieBasket
{
    [BsonId]
    public ObjectId Id { get; set; }

    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid UserId { get; set; }
    
    public ObjectId MovieId { get; set; }
}