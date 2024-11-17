using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MovieHub.Domain.BackgroundServices.CreateRegisteredUser;
using MovieHub.Storage.Storages;

namespace MovieHub.Storage.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddStorage(this IServiceCollection services, string dbContextConnectionString)
    {
        services.AddDbContextPool<MovieHubDbContext>(options => 
            options.UseNpgsql(dbContextConnectionString));
        
        services.AddScoped<ICreateSynchronizationUserStorage,CreateSynchronizationUserStorageStorage>();

        return services;
    }
}