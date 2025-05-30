using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Filters;
using Serilog.Sinks.OpenSearch;

namespace MovieHub.UserSync.KeycloakConsumer.Monitoring;

public static class LoginServiceCollectionExtension
{
    public static IServiceCollection AddLogin(this IServiceCollection services, IConfiguration configuration,
        IWebHostEnvironment environment)
    {
        var loggingLevelSwitch = new LoggingLevelSwitch
        {
            MinimumLevel = LogEventLevel.Information
        };
        services.AddSingleton(loggingLevelSwitch);

        services.AddLogging(a => a.AddSerilog(new LoggerConfiguration()
            .MinimumLevel.ControlledBy(loggingLevelSwitch)
            .Enrich.WithProperty("Application", Metrics.ApplicationName)
            .Enrich.WithProperty("Environment", environment.EnvironmentName)
            .WriteTo.Logger(lc => lc
                .Filter.ByExcluding(Matching.FromSource("Microsoft"))
                .WriteTo.OpenSearch(new OpenSearchSinkOptions(new Uri(configuration["Logs:ConnectionString"]!))
                {
                    IndexFormat = "UserSync.KeycloakConsumer-{0:yyyy.MM.dd}",
                    ModifyConnectionSettings = setting=> 
                        setting.BasicAuthentication(configuration["Logs:Username"],configuration["Logs:Password"])
                }))
            .WriteTo.Logger(lc => lc
                .WriteTo.Console())
            .CreateLogger()));

        return services;
    }
}