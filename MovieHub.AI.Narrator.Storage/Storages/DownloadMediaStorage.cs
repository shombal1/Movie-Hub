using Microsoft.Extensions.Options;
using MovieHub.AI.Narrator.Domain.UseCases.GenerateMovieDescription;

namespace MovieHub.AI.Narrator.Storage.Storages;

public class DownloadMediaStorage(
    IS3StorageService s3StorageService,
    IOptions<DownloadSettings> downloadSettings,
    IOptions<S3Settings> s3Settings) : IDownloadMediaStorage
{
    public Task<string> DownloadMedia(string key, CancellationToken cancellationToken)
    {
        return s3StorageService.DownloadMedia(s3Settings.Value.UploadsBucket, key,
            downloadSettings.Value.LocalStoragePath, cancellationToken);
    }
}