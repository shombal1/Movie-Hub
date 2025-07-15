using MovieHub.AI.Narrator.Integration;
using Quartz;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;


builder.Services.Configure<BackgroundJobOptions>(configuration.GetSection(BackgroundJobOptions.SectionName));

builder.Services.AddQuartz(q =>
{
    q.UseSimpleTypeLoader();
    q.UseDefaultThreadPool(tp => { tp.MaxConcurrency = 3; });
    
    q.UsePersistentStore(s =>
    {
        var connectionString = builder.Configuration["Quartz:ConnectionString"] 
                               ?? throw new InvalidOperationException("Connection string for Quartz is not configured.");;
        var tablePrefix = builder.Configuration["Quartz:TablePrefix"];

        s.UseProperties = true;
        s.RetryInterval = TimeSpan.FromSeconds(15);
        s.UsePostgres(configurer =>
        {
            configurer.ConnectionString = connectionString;
            if (!string.IsNullOrEmpty(tablePrefix))
                configurer.TablePrefix = tablePrefix;
        });
        s.UseNewtonsoftJsonSerializer();
        s.UseClustering(c =>
        {
            c.CheckinInterval = TimeSpan.FromSeconds(10);
            c.CheckinMisfireThreshold = TimeSpan.FromSeconds(20);
        });
    });
});

builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var downloadSettings = scope.ServiceProvider.GetRequiredService<IOptions<DownloadSettings>>();
    var localStoragePath = downloadSettings.Value.LocalStoragePath;

    if (string.IsNullOrWhiteSpace(localStoragePath))
    {
        localStoragePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp_storage");
    }

    if (!Directory.Exists(localStoragePath))
    {
        Directory.CreateDirectory(localStoragePath);
    }
    
    downloadSettings.Value.LocalStoragePath = localStoragePath;
}

app.Run();