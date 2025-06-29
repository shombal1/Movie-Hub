namespace MovieHub.MediaCompressor.Domain;

public interface IMovieCompressUseCase
{
    public Task Compress(string key, S3Record s3Record, CancellationToken cancellationToken);
}