using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MovieHub.Engine.Storage.Models;

namespace MovieHub.Engine.Storage.Entities;

public class SeasonEntity
{
    [BsonId]
    public Guid Id { get; set; }
    
    [BsonElement("seriesId")]
    public Guid SeriesId { get; set; }
    
    [BsonElement("number")]
    public int Number { get; set; }

    [BsonElement("releaseYear")]
    public int ReleaseYearAt { get; set; }

    [BsonElement("episodes")]
    public IEnumerable<Episode> Episodes { get; set; } = null!;
}