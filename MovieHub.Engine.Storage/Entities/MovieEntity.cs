using MongoDB.Bson.Serialization.Attributes;

namespace MovieHub.Engine.Storage.Entities;

[BsonDiscriminator("MovieEntity")]
public class MovieEntity : MediaEntity
{
    public string Quality { get; set; } = "";
}