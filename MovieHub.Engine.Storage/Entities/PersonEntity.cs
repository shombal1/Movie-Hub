using MongoDB.Bson.Serialization.Attributes;
using MovieHub.Engine.Storage.Common;

namespace MovieHub.Engine.Storage.Entities;

public class PersonEntity
{
    [BsonId]
    public Guid Id { get; set; }

    [BsonElement("fullName")]
    public string FullName { get; set; } = "";

    [BsonElement("photoUrl")]
    public string PhotoUrl { get; set; } = null!;

    [BsonElement("birthDate")]
    [BsonIgnoreIfNull]
    public DateOnly? BirthDate { get; set; }

    [BsonElement("professions")]
    public IEnumerable<ProfessionType> Professions { get; set; } = new List<ProfessionType>(); 

    [BsonElement("biography")]
    [BsonIgnoreIfNull]
    public string? Biography { get; set; }

    [BsonElement("mediaIds")] 
    public IEnumerable<Guid> MediaIds { get; set; } = new List<Guid>();
}