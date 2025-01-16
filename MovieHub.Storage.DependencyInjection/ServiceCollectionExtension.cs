using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using MovieHub.Domain;

namespace MovieHub.Storage.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddStorage(this IServiceCollection services, string contextConnectionString)
    {
        services.AddSingleton<IMongoClient>(_ => new MongoClient(contextConnectionString));

        services.AddScoped<MovieHubDbContext>();
        
        services.AddSingleton<IUnitOfWork, UnitOfWork>();
        
        return services;
    }
}