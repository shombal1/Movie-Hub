namespace MovieHub.AI.Narrator.Domain.UseCases.RetryFailedNarratorJob;

public interface IFailedNarratorMarkAsRetriedStorage
{
    public Task Mark(Guid jobId,CancellationToken cancellationToken);
}