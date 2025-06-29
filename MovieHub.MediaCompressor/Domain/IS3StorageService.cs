namespace MovieHub.MediaCompressor.Domain;

public interface IS3StorageService
{
    public Task<string> DownloadUploadMedia(string key, string destinationFolder,
        CancellationToken cancellationToken = default);

    public Task UploadProcessedMedia(string key, string filePath, int partSize = 5 * 1024 * 1024,
        CancellationToken cancellationToken = default);
}