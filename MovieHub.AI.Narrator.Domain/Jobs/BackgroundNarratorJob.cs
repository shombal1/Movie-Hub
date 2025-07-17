using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MovieHub.AI.Narrator.Domain.UseCases.GenerateMovieDescription;
using Quartz;

namespace MovieHub.AI.Narrator.Domain.Jobs;

public class BackgroundNarratorJob(
    IServiceScopeFactory scopeFactory,
    ILogger<BackgroundNarratorJob> logger)
    : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        var dataMap = context.JobDetail.JobDataMap;
        var s3Key = dataMap.GetString("s3Key");
        var mediaId = Guid.Parse(dataMap.GetString("movieRequestId") ?? Guid.Empty.ToString());

        try
        {
            await using var scope = scopeFactory.CreateAsyncScope();
            var provider = scope.ServiceProvider;
            var generateMediaDescriptionUseCase = provider.GetRequiredService<IGenerateMediaDescriptionUseCase>();
            
            var command = new GenerateMovieDescriptionCommand(s3Key!, mediaId);
            await generateMediaDescriptionUseCase.GenerateMediaDescription(
                command,
                context.CancellationToken);
            
            var deleteFailedNarratorJobStorage = provider.GetRequiredService<IDeleteFailedNarratorJobStorage>();
            await deleteFailedNarratorJobStorage.Delete(mediaId, context.CancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error generating description for key {Key}", s3Key);
            await using var scope = scopeFactory.CreateAsyncScope();
            var provider = scope.ServiceProvider;
            
            var storage = provider.GetRequiredService<ICreateFailedNarratorJobStorage>();
            var jobName = context.JobDetail.Key.Name;
            var jobGroup = context.JobDetail.Key.Group;
            string message = ErrorMessageConverterNarratorJob.ConvertToHumanReadableMessage(ex);
            
            await storage.Create(jobName, jobGroup, mediaId, s3Key, message);
            throw;
        }
    }
}