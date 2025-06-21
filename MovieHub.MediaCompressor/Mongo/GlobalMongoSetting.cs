using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace MovieHub.MediaCompressor.Mongo;

public static class GlobalMongoSetting
{
    public static void Configure()
    {
        BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
        
        BsonSerializer.RegisterSerializer(new EnumSerializer<QualityType>(BsonType.String));
    }
}