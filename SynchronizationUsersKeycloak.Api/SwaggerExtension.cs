using Microsoft.OpenApi.Models;

namespace SynchronizationUsersKeycloak.Api;

public static class SwaggerExtension
{
    public static IServiceCollection AddSwaggerGenWithAuth(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSwaggerGen(c =>
        {
            var securityScheme = new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows()
                {
                    Implicit = new OpenApiOAuthFlow()
                    {
                        AuthorizationUrl =
                            new Uri(
                                $"{configuration["Keycloak:auth-server-url"]}realms/{configuration["Keycloak:realm"]}/protocol/openid-connect/auth"),
                    }
                }
            };
            c.AddSecurityDefinition("Keycloak", securityScheme);

            var openApiSecurityRequirement = new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme()
                    {
                        Reference = new OpenApiReference()
                        {
                            Id = "Keycloak",
                            Type = ReferenceType.SecurityScheme
                        },
                        In = ParameterLocation.Header,
                        Name = "Bearer",
                        Scheme = "Bearer"
                    },
                    []
                }
            };

            c.AddSecurityRequirement(openApiSecurityRequirement);
        });

        return services;
    }
}