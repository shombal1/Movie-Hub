using Amazon.S3.Model;
using Microsoft.Extensions.Options;
using MovieHub.Engine.Domain.UseCases.AddMedia;
using MovieHub.Engine.Domain.UseCases.AddMedia.FinalizeMovieUpload;
using MovieHub.Engine.Storage.Common;

namespace MovieHub.Engine.Storage.Storages.AddMovie;

public class FinalizeMovieUploadStorage(
    IS3FileUploadService fileUploadService, 
    IOptions<S3Settings> s3Settings)
    : IFinalizeMovieUploadStorage
{
    public Task FinalizeMultiPartUpload(string uploadId, IEnumerable<FilePart> parts, string key,
        CancellationToken cancellationToken) =>
        fileUploadService.CompleteMultipartUpload(
            s3Settings.Value.UploadsBucket,
            uploadId,
            parts.Select(x => new PartETag(x.PartNumber, x.PartName)).ToList(),
            key,
            cancellationToken);
}