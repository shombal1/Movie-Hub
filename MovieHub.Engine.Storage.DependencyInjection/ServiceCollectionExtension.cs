using System.Reflection;
using Amazon.S3;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using MovieHub.Engine.Domain;
using MovieHub.Engine.Domain.Jobs.SyncMediaViews;
using MovieHub.Engine.Domain.UseCases;
using MovieHub.Engine.Domain.UseCases.AddMedia.InitiateMovieAddition;
using MovieHub.Engine.Domain.UseCases.AddMediaToBasket;
using MovieHub.Engine.Domain.UseCases.GetMedia;
using MovieHub.Engine.Domain.UseCases.GetMediaFromBasket;
using MovieHub.Engine.Domain.UseCases.GetMediaFullInfo;
using MovieHub.Engine.Domain.UseCases.IncrementMediaViews;
using MovieHub.Engine.Domain.UseCases.RemoveMediaFromBasket;
using MovieHub.Engine.Storage.Common;
using MovieHub.Engine.Storage.Mapping;
using MovieHub.Engine.Storage.Storages;
using MovieHub.Engine.Storage.Storages.AddMovie;
using StackExchange.Redis;

namespace MovieHub.Engine.Storage.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddStorage(
        this IServiceCollection services,
        string contextConnectionString,
        string cashConnectionString,
        string s3StorageConnectionString)
    {
        GlobalMongoSetting.Configure();
        
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
        services.AddScoped<IInitiateMovieAdditionStorage, InitiateMovieAdditionStorage>();
        
        services.AddScoped<IDomainEventStorage, DomainEventStorage>();
        
        services.AddScoped<ISeedGenerator, SeedGenerator>();
        
        services.AddSingleton<IUnitOfWork, UnitOfWork>();
        services.AddSingleton<IGuidFactory, GuidFactory>();
        services.AddSingleton<TimeProvider>(factory => TimeProvider.System);
        services.AddSingleton<IAmazonS3>(sp =>
        {
            var config = new AmazonS3Config
            {
                ServiceURL = s3StorageConnectionString.Split(';').First(x => x.StartsWith("Endpoint=")).Replace("Endpoint=", ""),
                ForcePathStyle = true,
                UseHttp = true
            };

            var client = new AmazonS3Client(
                s3StorageConnectionString.Split(';').First(x => x.StartsWith("AccessKey=")).Replace("AccessKey=", ""),
                s3StorageConnectionString.Split(';').First(x => x.StartsWith("SecretKey=")).Replace("SecretKey=", ""),
                config
            );

            return client;
        });
        services.AddSingleton<IS3FileUploadService, S3FileUploadService>();

        services.AddAutoMapper(conf => conf.AddMaps(Assembly.GetAssembly(typeof(MediaProfile))));
        
        return services;
    }
}