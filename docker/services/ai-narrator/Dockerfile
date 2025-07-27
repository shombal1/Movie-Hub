FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY MovieHub.AI.Narrator.Integration/ ./MovieHub.AI.Narrator.Integration/
COPY MovieHub.AI.Narrator.Domain/ ./MovieHub.AI.Narrator.Domain/
COPY MovieHub.AI.Narrator.Storage/ ./MovieHub.AI.Narrator.Storage

RUN dotnet restore MovieHub.AI.Narrator.Integration && \
    dotnet publish -c Debug -o out MovieHub.AI.Narrator.Integration


FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
RUN apt-get update && \
    apt-get install -y ffmpeg && \
    rm -rf /var/lib/apt/lists/*
COPY --from=build /app/out .

ENTRYPOINT ["dotnet", "MovieHub.AI.Narrator.Integration.dll"]