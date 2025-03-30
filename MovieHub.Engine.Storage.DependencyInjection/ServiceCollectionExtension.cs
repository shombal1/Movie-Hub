using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using MovieHub.Engine.Domain;
using MovieHub.Engine.Domain.Jobs.SyncMediaViews;
using MovieHub.Engine.Domain.UseCases.AddMediaToBasket;
using MovieHub.Engine.Domain.UseCases.GetMedia;
using MovieHub.Engine.Domain.UseCases.GetMediaFromBasket;
using MovieHub.Engine.Domain.UseCases.GetMediaFullInfo;
using MovieHub.Engine.Domain.UseCases.IncrementMediaViews;
using MovieHub.Engine.Domain.UseCases.RemoveMediaFromBasket;
using MovieHub.Engine.Storage.Mapping;
using MovieHub.Engine.Storage.Storages;
using StackExchange.Redis;

namespace MovieHub.Engine.Storage.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddStorage(
        this IServiceCollection services, 
        string contextConnectionString,
        string cashConnectionString)
    {
        BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
        
        
        services.AddSingleton<IMongoClient>(_ => new MongoClient(contextConnectionString));
        services.AddScoped<MovieHubDbContext>();
        
        services.AddSingleton<IConnectionMultiplexer>(
            ConnectionMultiplexer.Connect(cashConnectionString));
        services.AddScoped<IDatabase>(sp =>
        {
            var multiplexer = sp.GetRequiredService<IConnectionMultiplexer>();
            return multiplexer.GetDatabase();
        });
        
        services.AddScoped<IGetMediaStorage,GetMediaStorage>();
        services.AddScoped<ITryAddMediaToBasketStorage, TryAddMediaToBasketStorage>();
        services.AddScoped<IGetMediaFromBasketStorage,GetMediaFromBasketStorage>();
        services.AddScoped<IRemoveMediaFromBasketStorage, RemoveMediaFromBasketStorage>();
        services.AddScoped<IGetMediaIdsStorage, GetMediaIdsStorage>();
        services.AddScoped<IIncrementMediaViewsStorage,IncrementMediaViewsStorage>();
        services.AddScoped<IPushMediaViewsStorage, PushMediaViewsStorage>();
        services.AddScoped<IPopMediaViewsStorage, PopMediaViewsStorage>();
        services.AddScoped<IGetMediaFullInfoStorage, GetMediaFullInfoStorage>();
        
        services.AddSingleton<IUnitOfWork, UnitOfWork>();
        services.AddSingleton<IGuidFactory, GuidFactory>();

        services.AddAutoMapper(conf => conf.AddMaps(Assembly.GetAssembly(typeof(MediaProfile))));
        
        return services;
    }
}