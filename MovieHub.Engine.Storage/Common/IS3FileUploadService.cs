using Amazon.S3.Model;

namespace MovieHub.Engine.Storage.Common;

public interface IS3FileUploadService
{
    public Task<string> InitMultiPartUpload(string key, string contentType, Dictionary<string, string> metaData,
        CancellationToken cancellationToken);

    public Task<string> GeneratePresignedUrlForPart(string uploadId, int partNumber, string key,
        int expirationTimeMinutes, CancellationToken cancellationToken);

    public Task CompleteMultipartUpload(string uploadId, List<PartETag> parts, string key,
        CancellationToken cancellationToken);
}