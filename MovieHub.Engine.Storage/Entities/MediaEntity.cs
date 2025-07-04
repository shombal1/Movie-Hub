using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;

namespace MovieHub.Engine.Storage.Entities;

[BsonDiscriminator(RootClass = true)]
[BsonKnownTypes(typeof(SeriesEntity), typeof(MovieEntity))]
public abstract class MediaEntity
{
    [BsonId]
    public Guid Id { get; set; }

    [MaxLength(100)]
    [BsonElement("title")]
    public string Title { get; set; } = "";

    [MaxLength(1000)]
    [BsonElement("description")]
    public string Description { get; set; } = "";

    [BsonElement("releasedAt")]
    public DateOnly ReleasedAt { get; set; }

    [BsonElement("releasedYearAt")]
    public int ReleasedYearAt { get; set; }

    [BsonElement("publishedAt")]
    public DateTimeOffset PublishedAt { get; set; }

    [BsonElement("countries")]
    public IEnumerable<string> Countries { get; set; } = null!;

    [BsonElement("genres")]
    public IEnumerable<string> Genres { get; set; } = null!;
    
    
    [BsonElement("views")]
    public long Views { get; set; }
}