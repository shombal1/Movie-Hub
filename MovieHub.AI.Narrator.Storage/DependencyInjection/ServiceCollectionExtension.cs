using System.Reflection;
using Amazon.S3;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using MovieHub.AI.Narrator.Domain.Jobs;
using MovieHub.AI.Narrator.Domain.UseCases.GenerateMovieDescription;
using MovieHub.AI.Narrator.Domain.UseCases.GetFailedNarratorJobs;
using MovieHub.AI.Narrator.Domain.UseCases.RetryFailedNarratorJob;
using MovieHub.AI.Narrator.Storage.Mapping;
using MovieHub.AI.Narrator.Storage.Storages;
using Quartz;
using Quartz.Impl.AdoJobStore;
using Whisper.net.Ggml;

namespace MovieHub.AI.Narrator.Storage.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddStorageServices(
        this IServiceCollection services,
        string mongoDbConnectionString,
        string s3StorageConnectionString,
        string quartzPostgresConnectionString)
    {
        GlobalMongoSetting.Configure();

        services.AddSingleton<IMongoClient>(_ => new MongoClient(mongoDbConnectionString));
        services.AddSingleton<MovieHubDbContext>();

        services.AddSingleton<IAmazonS3>(sp =>
        {
            var config = new AmazonS3Config
            {
                ServiceURL =
                    s3StorageConnectionString.Split(';').First(x => x.StartsWith("Endpoint=")).Replace("Endpoint=", ""),
                ForcePathStyle = true,
                UseHttp = true
            };

            var client = new AmazonS3Client(
                s3StorageConnectionString.Split(';').First(x => x.StartsWith("AccessKey=")).Replace("AccessKey=", ""),
                s3StorageConnectionString.Split(';').First(x => x.StartsWith("SecretKey=")).Replace("SecretKey=", ""),
                config
            );

            return client;
        });

        services.AddQuartz(q =>
        {
            q.UseSimpleTypeLoader();
            q.UseDefaultThreadPool(tp => { tp.MaxConcurrency = 3; });

            q.UsePersistentStore(s =>
            {
                var connectionString = quartzPostgresConnectionString;

                s.PerformSchemaValidation = true;
                s.UseProperties = true;
                s.RetryInterval = TimeSpan.FromSeconds(15);
                s.UsePostgres(configurer =>
                {
                    configurer.ConnectionString = connectionString;
                    configurer.TablePrefix = "qrtz_";
                    configurer.UseDriverDelegate<PostgreSQLDelegate>();
                });
                s.UseNewtonsoftJsonSerializer();
                s.UseClustering(c =>
                {
                    c.CheckinInterval = TimeSpan.FromSeconds(10);
                    c.CheckinMisfireThreshold = TimeSpan.FromSeconds(20);
                });
            });
        });

        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

        services.AddDbContext<QuartzDbContext>(options =>
            options.UseNpgsql(quartzPostgresConnectionString));

        services.AddSingleton<IS3StorageService, S3StorageService>();

        services.AddHttpClient();

        services.AddSingleton<WhisperFactoryProvider>(provider => new WhisperFactoryProvider(GgmlType.Base));
        services.AddScoped<WhisperProcessorFactory>();

        services.AddSingleton<IGuidFactory, GuidFactory>();
        services.AddSingleton<TimeProvider>(x => TimeProvider.System);

        services.AddScoped<IDownloadMediaStorage, DownloadMediaStorage>();
        services.AddScoped<IExtractAudioStorage, ExtractAudioStorage>();
        services.AddScoped<IGenerateMediaDescriptionStorage, GenerateMediaDescriptionStorage>();
        services.AddScoped<IGetGeneratedDescriptionStorage, GetGeneratedDescriptionStorage>();
        services.AddScoped<IMediaTranscriptExtractorStorage, MediaTranscriptExtractorStorage>();
        services.AddScoped<ISetAiDescriptionStorage, SetAiDescriptionStorage>();
        services.AddScoped<ICreateFailedNarratorJobStorage, CreateFailedNarratorJobStorage>();
        services.AddScoped<IGetFailedNarratorJobStorage, GetFailedNarratorJobStorage>();
        services.AddScoped<IFailedNarratorMarkAsRetriedStorage, FailedNarratorMarkAsRetriedStorage>();
        services.AddScoped<IDeleteFailedNarratorJobStorage, DeleteFailedNarratorJobStorage>();
        
        services.AddAutoMapper(conf => conf.AddMaps(Assembly.GetAssembly(typeof(FailedNarratorJobProfile))));

        return services;
    }
}