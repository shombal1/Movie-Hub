using MongoDB.Bson.Serialization.Attributes;

namespace MovieHub.Engine.Storage.Entities;

[BsonDiscriminator("additionSeriesInfo")]
public class AdditionSeriesInfoEntity : AdditionMediaInfoEntity
{

    [BsonElement("seasons")] 
    [BsonIgnoreIfNull]
    public IEnumerable<SeasonEntity> Seasons { get; set; } = null!;
}