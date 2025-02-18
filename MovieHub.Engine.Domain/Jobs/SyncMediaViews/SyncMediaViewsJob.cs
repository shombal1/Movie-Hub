using Quartz;

namespace MovieHub.Engine.Domain.Jobs.SyncMediaViews;


public class SyncMediaViewsJob(
    IGetMediaIdsStorage getMediaIdsStorage,
    IPopMediaViewsStorage popMediaViewsStorage,
    IPushMediaViewsStorage pushMediaViewsStorage) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        var cancellationToken = context.CancellationToken;
        
        var mediaIds = await getMediaIdsStorage.Get(cancellationToken);
        
        var viewsCount = await Task.WhenAll(mediaIds
            .Select(async mediaId => (
                mediaId: mediaId,
                views: await popMediaViewsStorage.PopViews(mediaId, cancellationToken))));
        
        await pushMediaViewsStorage.Increment(viewsCount, cancellationToken);
    }
}