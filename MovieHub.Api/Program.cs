using Confluent.Kafka;
using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Authorization;
using Keycloak.AuthServices.Common;
using Microsoft.Extensions.Options;
using MovieHub.Api;
using MovieHub.Api.Middleware;
using MovieHub.Domain.BackgroundServices.CreateRegisteredUser;
using MovieHub.Domain.DependencyInjection;
using MovieHub.Storage.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager configuration = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDomain();
builder.Services.AddStorage(configuration.GetConnectionString("MovieHubDbContext")!);

builder.Services.Configure<CreateRegisteredUserConsumerConfig>(configuration
    .GetSection("KafkaCreateRegisteredUserConsumer").Bind);
builder.Services.AddSingleton(sp => new ConsumerBuilder<byte[], byte[]>(
    sp.GetRequiredService<IOptions<CreateRegisteredUserConsumerConfig>>().Value).Build());

builder.Services.AddKeycloakWebApiAuthentication(
    configuration,
    options =>
    {
        options.RequireHttpsMetadata = false;
        options.Audience = "account";
    });

builder.Services.AddAuthorization();
builder.Services.AddKeycloakAuthorization(options =>
{
    options.EnableRolesMapping = RolesClaimTransformationSource.Realm;
    options.RoleClaimType = KeycloakConstants.RoleClaimType;
});

builder.Services.AddSwaggerGenWithAuth(configuration);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options => options.OAuthClientId(builder.Configuration["Keycloak:resource"]!));

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<IdentityMiddleware>();

app.MapControllers();

app.Run();