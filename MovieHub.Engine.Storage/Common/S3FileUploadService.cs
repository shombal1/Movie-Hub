using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Options;

namespace MovieHub.Engine.Storage.Common;

public class S3FileUploadService(
    IAmazonS3 s3Client,
    TimeProvider timeProvider) : IS3FileUploadService
{

    public async Task<string> InitMultiPartUpload(string bucketName, string key,string contentType,Dictionary<string,string> metaData, CancellationToken cancellationToken)
    {
        var request = new InitiateMultipartUploadRequest
        {
            BucketName = bucketName,
            Key = key,
            ContentType = contentType,
        };

        foreach (var (keyMetaData,valueMetaData) in metaData)
        {
            request.Metadata.Add(keyMetaData, valueMetaData);   
        }
        
        var response = await s3Client.InitiateMultipartUploadAsync(request, cancellationToken);
        return response.UploadId;
    }

    public async Task<string> GeneratePresignedUrlForPart(string bucketName, string uploadId, int partNumber, string key,
        int expirationTimeMinutes, CancellationToken cancellationToken)
    {
        var now = timeProvider.GetUtcNow();
        var request = new GetPreSignedUrlRequest
        {
            BucketName = bucketName,
            Key = key,
            Verb = HttpVerb.PUT,
            Expires = now.UtcDateTime.AddMinutes(expirationTimeMinutes),
            PartNumber = partNumber,
            UploadId = uploadId,
        };

        string url =  await s3Client.GetPreSignedURLAsync(request);
        return url;
    }

    public async Task CompleteMultipartUpload(string bucketName, string uploadId, List<PartETag> parts, string key,
        CancellationToken cancellationToken)
    {
        var request = new CompleteMultipartUploadRequest
        {
            BucketName = bucketName,
            Key = key,
            UploadId = uploadId,
            PartETags = parts,
        };

        await s3Client.CompleteMultipartUploadAsync(request, cancellationToken);
    }
}