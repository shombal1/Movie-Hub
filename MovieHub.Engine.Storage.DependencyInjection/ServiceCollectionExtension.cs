using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MovieHub.Engine.Domain;
using MovieHub.Engine.Domain.UseCases.GetMedia;
using MovieHub.Engine.Storage.Entities;
using MovieHub.Engine.Storage.Storages;

namespace MovieHub.Engine.Storage.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddStorage(this IServiceCollection services, string contextConnectionString)
    {
        BsonClassMap.RegisterClassMap<MediaEntity>(cm =>
        {
            cm.AutoMap();
            cm.SetIsRootClass(true);
        });
        BsonClassMap.RegisterClassMap<MovieEntity>(m => m.AutoMap());
        BsonClassMap.RegisterClassMap<SeriesEntity>(m => m.AutoMap());
        
        services.AddSingleton<IMongoClient>(_ => new MongoClient(contextConnectionString));
        services.AddScoped<MovieHubDbContext>();
        
        services.AddScoped<IGetMediaStorage,GetMediaStorage>();
        
        services.AddSingleton<IUnitOfWork, UnitOfWork>();

        services.AddStorageMapster();
        
        return services;
    }
}