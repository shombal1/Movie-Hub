using MovieHub.MediaCompressor.Mongo;

namespace MovieHub.MediaCompressor.Domain;

public interface IGetProcessingStatusStorage
{
    public Task<ProcessingStatus?> GetFromMovieRequest(Guid movieRequestId,CancellationToken cancellationToken);
}