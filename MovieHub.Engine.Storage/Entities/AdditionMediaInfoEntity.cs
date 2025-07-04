using MongoDB.Bson.Serialization.Attributes;
using MovieHub.Engine.Storage.Models;

namespace MovieHub.Engine.Storage.Entities;

[BsonDiscriminator(RootClass = true)]
[BsonKnownTypes(typeof(AdditionMovieInfoEntity), typeof(AdditionSeriesInfoEntity))]
public abstract class AdditionMediaInfoEntity
{
    [BsonId]
    [BsonElement("mediaId")]
    public Guid MediaId { get; set; }
    
    [BsonElement("actors")]
    public IEnumerable<BasePersonInfo> Actors { get; set; }= null!;
    [BsonElement("directors")]
    public IEnumerable<BasePersonInfo> Directors { get; set; } = null!;

    [BsonElement("ageRating")]
    public string AgeRating { get; set; } = "";
    
    [BsonElement("budget")]
    public long? Budget { get; set; }
    
    // used for request to get full media info
    [BsonElement("mainInfoList")]
    [BsonIgnoreIfNull]
    public IEnumerable<MediaEntity> MainInfoList { get; set; } = null!;
    
    [BsonElement("mainInfo")]
    [BsonIgnoreIfNull]
    public MediaEntity MainInfo { get; set; } = null!;
}