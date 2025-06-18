FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY MovieHub.Engine.Api/ ./MovieHub.Engine.Api/
COPY MovieHub.Engine.Domain/ ./MovieHub.Engine.Domain/
COPY MovieHub.Engine.Domain.DependencyInjection/ ./MovieHub.Engine.Domain.DependencyInjection/
COPY MovieHub.Engine.Storage/ ./MovieHub.Engine.Storage/
COPY MovieHub.Engine.Storage.DependencyInjection/ ./MovieHub.Engine.Storage.DependencyInjection/

RUN dotnet restore MovieHub.Engine.Api && \
    dotnet publish -c Debug -o out MovieHub.Engine.Api


FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

ENTRYPOINT ["dotnet", "MovieHub.Engine.Api.dll"]