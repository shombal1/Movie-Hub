using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using MovieHub.Domain;
using MovieHub.Domain.BackgroundServices.CreateRegisteredUser;
using MovieHub.Storage.Storages;

namespace MovieHub.Storage.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddStorage(this IServiceCollection services, string contextConnectionString)
    {
        services.AddSingleton<IMongoClient>(_ => new MongoClient(contextConnectionString));

        services.AddScoped<MovieHubDbContext>();
        
        services.AddScoped<ICreateSynchronizationUserStorage,CreateSynchronizationUserStorageStorage>();

        services.AddSingleton<IUnitOfWork, UnitOfWork>();
        
        return services;
    }
}