using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MovieHub.Engine.Storage.Common;

namespace MovieHub.Engine.Storage;

public static class GlobalMongoSetting
{
    public static void Configure()
    {
        BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
    }
}