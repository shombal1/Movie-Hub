using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MovieHub.Storage.Entities;

public class Movie
{
    [BsonId]
    public ObjectId Id { get; set; }
    [MaxLength(100)] public string Title { get; set; } = "";
    [MaxLength(1000)] public string Description { get; set; } = "";
    public DateOnly ReleasedAt { get; set; }
}