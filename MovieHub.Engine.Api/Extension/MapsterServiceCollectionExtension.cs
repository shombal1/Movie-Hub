using System.Reflection;
using Mapster;
using MovieHub.Engine.Api.Mapper;
using MovieHub.Engine.Storage.Mapping;

namespace MovieHub.Engine.Api.Extension;

public static class MapsterServiceCollectionExtension
{
    public static IServiceCollection AddApiMapster(this IServiceCollection services)
    {
        var typeAdapterConfig = TypeAdapterConfig.GlobalSettings;
        typeAdapterConfig.AllowImplicitSourceInheritance = true;
        typeAdapterConfig.AllowImplicitDestinationInheritance = true;
        Assembly applicationAssembly = Assembly.GetAssembly(typeof(MediaRegistry))!;
        typeAdapterConfig.Scan(applicationAssembly);

        services.AddMapster();

        return services;
    }
}