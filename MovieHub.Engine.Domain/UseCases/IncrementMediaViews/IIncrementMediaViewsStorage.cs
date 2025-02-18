namespace MovieHub.Engine.Domain.UseCases.IncrementMediaViews;

public interface IIncrementMediaViewsStorage 
{
    public Task Increment(Guid mediaId,CancellationToken cancellationToken);
}