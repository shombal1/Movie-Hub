using MongoDB.Bson.Serialization.Attributes;

namespace MovieHub.Engine.Storage.Entities;

[BsonDiscriminator("movie")]
public class MovieEntity : MediaEntity
{

}