namespace MovieHub.AI.Narrator.Domain.UseCases.RetryFailedNarratorJob;

public interface IRetryFailedNarratorJobUseCase
{
    public Task RetryFailedJob(RetryFailedNarratorJobCommand request, CancellationToken cancellationToken);
}