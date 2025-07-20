using MongoDB.Bson.Serialization.Attributes;
using MovieHub.Engine.Storage.Common;

namespace MovieHub.Engine.Storage.Entities;

[BsonDiscriminator("additionMovieInfo")]
public class AdditionMovieInfoEntity : AdditionMediaInfoEntity
{
    [BsonElement("aiDescription")]
    public string AiDescription { get; set; } = "";
    
    // s3Key
    [BsonElement("availableQualities")] 
    public Dictionary<QualityType, string> AvailableQualities { get; set; } = new();
}