using Microsoft.Extensions.DependencyInjection;
using MovieHub.Engine.Domain.Authentication;

namespace MovieHub.Engine.Domain.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        services.AddScoped<IIdentityProvider, IdentityProvider>();

        // services.AddHostedService<CreateRegisteredUserConsumer>(c =>
        // {
        //     var scope = c.CreateScope();
        //     var serviceProvider = scope.ServiceProvider;
        //
        //     return new CreateRegisteredUserConsumer(
        //         serviceProvider.GetRequiredService<IConsumer<byte[], byte[]>>(),
        //         serviceProvider.GetRequiredService<ICreateSynchronizationUserStorage>());
        // });

        return services;
    }
}