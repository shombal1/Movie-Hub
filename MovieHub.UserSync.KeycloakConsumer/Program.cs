using Confluent.Kafka;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MovieHub.UserSync.KeycloakConsumer;
using MovieHub.UserSync.KeycloakConsumer.Monitoring;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

builder.Services.Configure<MongoDbConfigure>(builder.Configuration.GetSection("MongoDbConfigure").Bind);

builder.Services.AddSingleton<IMongoClient>(_ => new MongoClient( new MongoClientSettings()
{
    Server = MongoServerAddress.Parse(builder.Configuration["MongoDbConfigure:ConnectionString"]),
    WriteConcern = WriteConcern.W2
}));

builder.Services.AddSingleton<MongoDbContext>();

builder.Services.Configure<ConsumerConfig>(configuration
    .GetSection("Kafka").Bind);
builder.Services.Configure<KafkaTopic>(configuration
    .GetSection("KafkaTopic").Bind);
builder.Services.AddSingleton(sp => new ConsumerBuilder<byte[], byte[]>(
    sp.GetRequiredService<IOptions<ConsumerConfig>>().Value).Build());
builder.Services.AddSingleton<ICreateUserStorage, CreateUserStorage>();

builder.Services.AddHostedService<KeycloakEventsConsumer>();

builder.Services.AddTracing(configuration);
builder.Services.AddLogin(configuration,builder.Environment);

var app = builder.Build();

app.Run();