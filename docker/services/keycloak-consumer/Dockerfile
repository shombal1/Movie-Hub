FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY MovieHub.UserSync.KeycloakConsumer/ ./MovieHub.UserSync.KeycloakConsumer/

RUN dotnet restore MovieHub.UserSync.KeycloakConsumer && \
    dotnet publish -c Debug -o out MovieHub.UserSync.KeycloakConsumer


FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

ENTRYPOINT ["dotnet", "MovieHub.UserSync.KeycloakConsumer.dll"]