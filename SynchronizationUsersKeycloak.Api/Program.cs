
using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Authorization;
using Keycloak.AuthServices.Common;
using SynchronizationUsersKeycloak.Api;

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager configuration = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();


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
app.UseSwaggerUI(options=>options.OAuthClientId(builder.Configuration["Keycloak:resource"]!));

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
