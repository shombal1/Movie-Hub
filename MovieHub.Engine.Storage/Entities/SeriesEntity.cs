using MongoDB.Bson.Serialization.Attributes;

namespace MovieHub.Engine.Storage.Entities;


[BsonDiscriminator("series")]
public class SeriesEntity : MediaEntity
{
    [BsonElement("countSeasons")]
    public int CountSeasons { get; set; }

    [BsonElement("countEpisodes")]
    public int CountEpisodes { get; set; }
}