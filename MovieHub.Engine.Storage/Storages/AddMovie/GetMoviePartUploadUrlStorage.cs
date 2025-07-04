using Microsoft.Extensions.Options;
using MovieHub.Engine.Domain.UseCases.AddMedia.GetMoviePartUploadUrl;
using MovieHub.Engine.Storage.Common;

namespace MovieHub.Engine.Storage.Storages.AddMovie;

public class GetMoviePartUploadUrlStorage(IS3FileUploadService fileUploadService, IOptions<S3Settings> s3Settings)
    : IGetMoviePartUploadUrlStorage
{
    public Task<string> GeneratePresignedUrlForPart(string uploadId, int partNumber, string key,
        int expirationTimeMinutes, CancellationToken cancellationToken) =>
        fileUploadService.GeneratePresignedUrlForPart(
            s3Settings.Value.UploadsBucket,
            uploadId,
            partNumber,
            key,
            expirationTimeMinutes,
            cancellationToken);
}