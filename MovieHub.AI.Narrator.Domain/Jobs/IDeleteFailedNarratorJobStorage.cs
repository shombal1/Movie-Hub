namespace MovieHub.AI.Narrator.Domain.Jobs;

public interface IDeleteFailedNarratorJobStorage
{
    public Task Delete(Guid mediaId, CancellationToken cancellationToken);
}