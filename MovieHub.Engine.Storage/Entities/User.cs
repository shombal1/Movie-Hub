using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MovieHub.Engine.Storage.Entities;

public class User
{
    [BsonId]
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid Id { get; set; }
}