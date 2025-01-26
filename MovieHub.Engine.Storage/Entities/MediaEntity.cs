using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MovieHub.Engine.Storage.Entities;

public abstract class MediaEntity
{
    [BsonId]
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid Id { get; set; }
    [MaxLength(100)] public string Title { get; set; } = "";
    [MaxLength(1000)] public string Description { get; set; } = "";
    public DateOnly ReleasedAt { get; set; }
    public int ReleasedYearAt { get; set; }
    public DateTimeOffset PublishedAt { get; set; }
    public IEnumerable<string> Countries { get; set; } = null!;
    public IEnumerable<string> Genres { get; set; } = null!;
    public IEnumerable<string> Director { get; set; } = null!;
}