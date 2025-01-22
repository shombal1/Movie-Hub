using MongoDB.Bson.Serialization.Attributes;

namespace MovieHub.Engine.Storage.Entities;


[BsonDiscriminator("SeriesEntity")]
public class SeriesEntity: MediaEntity
{
    public int CountSeasons { get; set; }
    public int CountEpisodes { get; set; }
}