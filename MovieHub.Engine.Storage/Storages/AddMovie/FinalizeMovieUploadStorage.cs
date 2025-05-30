using Amazon.S3.Model;
using MovieHub.Engine.Domain.UseCases.AddMedia;
using MovieHub.Engine.Domain.UseCases.AddMedia.FinalizeMovieUpload;
using MovieHub.Engine.Storage.Common;

namespace MovieHub.Engine.Storage.Storages.AddMovie;

public class FinalizeMovieUploadStorage(IS3FileUploadService fileUploadService)
    : IFinalizeMovieUploadStorage
{
    public Task FinalizeMultiPartUpload(string uploadId, IEnumerable<FilePart> parts, string key,
        CancellationToken cancellationToken) =>
        fileUploadService.CompleteMultipartUpload(
            uploadId,
            parts.Select(x => new PartETag(x.PartNumber, x.PartName)).ToList(),
            key,
            cancellationToken);
}