namespace MovieHub.MediaCompressor.Domain;

public interface IUpdateProcessingStatusStorage
{
    public Task Update(Guid movieId, Dictionary<QualityType, string> processedQualities, CancellationToken cancellationToken);
}