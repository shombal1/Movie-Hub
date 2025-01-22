using Microsoft.Extensions.DependencyInjection;
using MovieHub.Engine.Domain.Authentication;

namespace MovieHub.Engine.Domain.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        services.AddScoped<IIdentityProvider, IdentityProvider>();

        services.AddMediatR(conf => conf.RegisterServicesFromAssembly(typeof(IUnitOfWork).Assembly));

        return services;
    }
}