using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MovieHub.Storage.Entities;

public class User
{
    [BsonId]
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid Id { get; set; }
}