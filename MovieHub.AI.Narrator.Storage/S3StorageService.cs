using Amazon.S3;
using Amazon.S3.Model;

namespace MovieHub.AI.Narrator.Storage;

public class S3StorageService(IAmazonS3 s3Client) : IS3StorageService
{
    public async Task<string> DownloadMedia(
        string bucketName, string key, string destinationFolder, CancellationToken cancellationToken)
    {
        string fileName = Path.GetFileName(key);

        string destinationPath = Path.Combine(destinationFolder, fileName);

        var request = new GetObjectRequest
        {
            BucketName = bucketName,
            Key = key
        };

        using var response = await s3Client.GetObjectAsync(request, cancellationToken);
        await using var responseStream = response.ResponseStream;
        await using var fileStream = File.Create(destinationPath);

        await responseStream.CopyToAsync(fileStream, cancellationToken);

        return destinationPath;
    }
}