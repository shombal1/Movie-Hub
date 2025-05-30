using MongoDB.Bson.Serialization.Attributes;

namespace MovieHub.Engine.Storage.Entities;

public class UserEntity
{
    [BsonId]
    public Guid Id { get; set; }
}