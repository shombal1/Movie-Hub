namespace MovieHub.Engine.Domain.UseCases.AddMedia.GetMoviePartUploadUrl;

public interface IGetMoviePartUploadUrlStorage
{
    public Task<string> GeneratePresignedUrlForPart(string uploadId, int partNumber, string key,
        int expirationTimeMinutes, CancellationToken cancellationToken);
}