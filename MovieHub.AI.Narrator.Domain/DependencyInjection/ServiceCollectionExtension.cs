using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using MovieHub.AI.Narrator.Domain.UseCases.GenerateMovieDescription;
using MovieHub.AI.Narrator.Domain.UseCases.GetFailedNarratorJobs;
using MovieHub.AI.Narrator.Domain.UseCases.RetryFailedNarratorJob;

namespace MovieHub.AI.Narrator.Domain.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetAssembly(typeof(IGenerateMediaDescriptionUseCase)), includeInternalTypes: true);
        
        services.AddScoped<IGenerateMediaDescriptionUseCase, GenerateMediaDescriptionUseCase>();
        services.AddScoped<IGetFailedNarratorJobsUseCase, GetFailedNarratorJobsUseCase>();
        services.AddScoped<IRetryFailedNarratorJobUseCase, RetryFailedNarratorJobUseCase>();
        
        return services;
    }
}