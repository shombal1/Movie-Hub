using MovieHub.Engine.Domain.UseCases.AddMedia.GetMoviePartUploadUrl;
using MovieHub.Engine.Storage.Common;

namespace MovieHub.Engine.Storage.Storages.AddMovie;

public class GetMoviePartUploadUrlStorage(IS3FileUploadService fileUploadService)
    : IGetMoviePartUploadUrlStorage
{
    public Task<string> GeneratePresignedUrlForPart(string uploadId, int partNumber, string key,
        int expirationTimeMinutes, CancellationToken cancellationToken) =>
        fileUploadService.GeneratePresignedUrlForPart(
            uploadId,
            partNumber,
            key,
            expirationTimeMinutes,
            cancellationToken);
}