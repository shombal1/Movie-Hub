using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MovieHub.Engine.Domain;
using MovieHub.Engine.Domain.UseCases.AddMediaToBasket;
using MovieHub.Engine.Domain.UseCases.GetMedia;
using MovieHub.Engine.Domain.UseCases.GetMediaFromBasket;
using MovieHub.Engine.Domain.UseCases.RemoveMediaFromBasket;
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
        services.AddScoped<ITryAddMediaToBasketStorage, TryAddMediaToBasketStorage>();
        services.AddScoped<IGetMediaFromBasketStorage,GetMediaFromBasketStorage>();
        services.AddScoped<IRemoveMediaFromBasketStorage, RemoveMediaFromBasketStorage>();
        
        services.AddSingleton<IUnitOfWork, UnitOfWork>();
        services.AddSingleton<IGuidFactory, GuidFactory>();

        services.AddStorageMapster();
        
        return services;
    }
}