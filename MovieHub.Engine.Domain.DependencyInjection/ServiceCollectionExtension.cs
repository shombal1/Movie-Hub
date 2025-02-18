using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using MovieHub.Engine.Domain.Authentication;
using MovieHub.Engine.Domain.Jobs.SyncMediaViews;
using Quartz;

namespace MovieHub.Engine.Domain.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        services.AddScoped<IIdentityProvider, IdentityProvider>();

        services.AddMediatR(conf => conf.RegisterServicesFromAssembly(typeof(IUnitOfWork).Assembly));

        services.AddValidatorsFromAssembly(Assembly.GetAssembly(typeof(IUnitOfWork)), includeInternalTypes: true);
        
         services.AddQuartz(cfg =>
         {
             var jobKey = new JobKey("SyncMovieViewsJob");
        
             cfg.AddJob<SyncMediaViewsJob>(opts => opts.WithIdentity(jobKey));
        
             cfg.AddTrigger(opts => opts
                 .ForJob(jobKey)
                 .WithIdentity("SyncMovieViewsTrigger")
                 .WithSimpleSchedule(schedule =>
                     schedule.WithIntervalInSeconds(10)
                         .RepeatForever()));
         });

        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

        services.AddTransient<SyncMediaViewsJob>();
        
        return services;
    }
}