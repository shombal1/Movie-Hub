using MongoDB.Bson.Serialization.Attributes;
using MovieHub.Engine.Storage.Common;

namespace MovieHub.Engine.Storage.Models;

public class BasePersonInfo
{
    [BsonId]
    public Guid Id { get; set; }

    [BsonElement("fullName")]
    public string FullName { get; set; } = "";

    [BsonElement("photoUrl")]
    public string PhotoUrl { get; set; } = null!;

    [BsonElement("professions")]
    public IEnumerable<ProfessionType> Professions { get; set; } = new List<ProfessionType>(); 
}