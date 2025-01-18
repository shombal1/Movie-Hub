using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace MovieHub.UserSync.KeycloakConsumer.Monitoring;

public static class TracingServiceCollectionExtension
{
    public static IServiceCollection AddTracing(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOpenTelemetry()
            .WithTracing(builder => builder
                .ConfigureResource(conf => conf.AddService("MovieHub.UserSync.KeycloakConsumer"))
                .AddSource(Metrics.ApplicationName)
                .AddJaegerExporter(conf =>
                    conf.Endpoint = new Uri(configuration.GetConnectionString("Tracing")!)));

        return services;
    }
}