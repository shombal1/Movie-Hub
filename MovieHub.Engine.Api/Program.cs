using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Authorization;
using Keycloak.AuthServices.Common;
using MovieHub.Engine.Api;
using MovieHub.Engine.Api.Middleware;
using MovieHub.Engine.Domain.DependencyInjection;
using MovieHub.Engine.Storage;
using MovieHub.Engine.Storage.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager configuration = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDomain();
builder.Services.Configure<MongoDbConfigure>(builder.Configuration.GetSection("MongoDbConfigure").Bind);
builder.Services.AddStorage(configuration.GetConnectionString("MovieHubMongoDb")!);


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