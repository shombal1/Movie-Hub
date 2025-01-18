using System.Diagnostics;

namespace MovieHub.UserSync.KeycloakConsumer.Monitoring;

public class Metrics
{
    public const string ApplicationName = "MovieHub.UserSync.KeycloakConsumer";
    public static readonly ActivitySource ActivitySource = new ActivitySource(ApplicationName);
}