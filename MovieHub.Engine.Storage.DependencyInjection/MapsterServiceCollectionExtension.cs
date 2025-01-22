using System.Reflection;
using Mapster;
using Microsoft.Extensions.DependencyInjection;
using MovieHub.Engine.Storage.Mapping;

namespace MovieHub.Engine.Storage.DependencyInjection;

internal static class MapsterServiceCollectionExtension
{
    public static IServiceCollection AddStorageMapster(this IServiceCollection services)
    {
        var typeAdapterConfig = TypeAdapterConfig.GlobalSettings;
        typeAdapterConfig.AllowImplicitSourceInheritance = true;
        typeAdapterConfig.AllowImplicitDestinationInheritance = true;
        Assembly applicationAssembly = Assembly.GetAssembly(typeof(StorageRegistry))!;
        typeAdapterConfig.Scan(applicationAssembly);

        services.AddMapster();

        return services;
    }
}