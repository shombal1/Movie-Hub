using MongoDB.Bson.Serialization.Attributes;

namespace MovieHub.Engine.Storage.Entities;

[BsonDiscriminator(RootClass = true)]
[BsonKnownTypes(typeof(AdditionMovieInfoEntity), typeof(AdditionSeriesInfoEntity))]
public abstract class AdditionMediaInfoEntity
{
    [BsonId]
    [BsonElement("mediaId")]
    public Guid MediaId { get; set; }
    
    [BsonElement("actors")]
    public IEnumerable<string> Actors { get; set; }= null!;

    [BsonElement("ageRating")]
    public string AgeRating { get; set; } = "";
    
    [BsonElement("budget")]
    public long? Budget { get; set; }
    
    [BsonElement("mainInfoList")]
    [BsonIgnoreIfNull]
    public IEnumerable<MediaEntity> MainInfoList { get; set; } = null!;
    
    [BsonElement("mainInfo")]
    [BsonIgnoreIfNull]
    public MediaEntity MainInfo { get; set; } = null!;
}