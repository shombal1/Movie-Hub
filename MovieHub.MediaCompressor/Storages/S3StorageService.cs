using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Options;
using MovieHub.MediaCompressor.Domain;

namespace MovieHub.MediaCompressor.Storages;

public class S3StorageService(IAmazonS3 s3Client, IOptions<S3Settings> s3Settings) : IS3StorageService
{
    private readonly S3Settings _s3Bucket = s3Settings.Value;

    public async Task<string> DownloadUploadMedia(string key, string destinationFolder,
        CancellationToken cancellationToken = default)
    {
        Directory.CreateDirectory(destinationFolder);

        string fileName = Path.GetFileName(key);

        string destinationPath = Path.Combine(destinationFolder, fileName);

        var request = new GetObjectRequest
        {
            BucketName = _s3Bucket.UploadsBucket,
            Key = key
        };

        using var response = await s3Client.GetObjectAsync(request, cancellationToken);
        await using var responseStream = response.ResponseStream;
        await using var fileStream = File.Create(destinationPath);

        await responseStream.CopyToAsync(fileStream, cancellationToken);

        return destinationPath;
    }

    public async Task UploadProcessedMedia(string key, string filePath, int partSize = 5 * 1024 * 1024,
        CancellationToken cancellationToken = default)
    {
        var initiateRequest = new InitiateMultipartUploadRequest
        {
            BucketName = _s3Bucket.ProcessedBucket,
            Key = key
        };

        var initiateResponse = await s3Client.InitiateMultipartUploadAsync(initiateRequest, cancellationToken);
        string uploadId = initiateResponse.UploadId;

        try
        {
            var fileInfo = new FileInfo(filePath);
            long fileSize = fileInfo.Length;
            long position = 0;
            int partNumber = 1;
            var uploadParts = new List<PartETag>();

            await using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                while (position < fileSize)
                {
                    int size = (int)Math.Min(partSize, fileSize - position);
                    byte[] buffer = new byte[size];
                    await fileStream.ReadExactlyAsync(buffer, 0, size, cancellationToken);

                    using (var partStream = new MemoryStream(buffer))
                    {
                        var uploadRequest = new UploadPartRequest
                        {
                            BucketName = _s3Bucket.ProcessedBucket,
                            Key = key,
                            UploadId = uploadId,
                            PartNumber = partNumber,
                            PartSize = size,
                            InputStream = partStream
                        };

                        var uploadResponse = await s3Client.UploadPartAsync(uploadRequest, cancellationToken);
                        uploadParts.Add(new PartETag(partNumber, uploadResponse.ETag));
                    }

                    position += size;
                    partNumber++;
                }
            }

            var completeRequest = new CompleteMultipartUploadRequest
            {
                BucketName = _s3Bucket.ProcessedBucket,
                Key = key,
                UploadId = uploadId,
                PartETags = uploadParts
            };

            await s3Client.CompleteMultipartUploadAsync(completeRequest, cancellationToken);
        }
        catch (Exception ex)
        {
            var abortRequest = new AbortMultipartUploadRequest
            {
                BucketName = _s3Bucket.ProcessedBucket,
                Key = key,
                UploadId = uploadId
            };

            await s3Client.AbortMultipartUploadAsync(abortRequest, cancellationToken);
            throw;
        }
    }
}