namespace MovieHub.AI.Narrator.Storage;

public interface IS3StorageService
{
    public Task<string> DownloadMedia(
        string bucketName, string key, string destinationFolder, CancellationToken cancellationToken);
}