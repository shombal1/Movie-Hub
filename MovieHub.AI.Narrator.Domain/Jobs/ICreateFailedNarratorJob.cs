namespace MovieHub.AI.Narrator.Domain.Jobs;

public interface ICreateFailedNarratorJob
{
    public Task<Guid> Create(
        string jobName,
        string jobGroup,
        Guid mediaId,
        string? s3Key,
        string errorMessage);
}