using Amazon.S3;
using Amazon.S3.Model;
using FluentAssertions;
using Microsoft.Extensions.Options;
using MovieHub.Engine.Storage.Common;

namespace MovieHub.Engine.Storage.Tests.S3FileUploadServiceTests;

public class S3FileUploadServiceShould(StorageTestFixture fixture) : IClassFixture<StorageTestFixture>, IAsyncLifetime
{
    private IAmazonS3 _s3Client;
    private S3FileUploadService _sut;
    private S3Settings _s3Settings;

    private const string TestBucketName = "test-bucket";
    private const int PartSizeMb = 50;
    private const int TotalSizeMb = 350;
    private const int PartSize = PartSizeMb * 1024 * 1024;
    private const int TotalSize = TotalSizeMb * 1024 * 1024;
    private const int UrlExpirationMinutes = 60;

    public async Task InitializeAsync()
    {
        _s3Client = await fixture.GetAmazonS3();
        _s3Settings = new S3Settings { UploadsBucket = TestBucketName };
        await _s3Client.PutBucketAsync(new PutBucketRequest { BucketName = _s3Settings.UploadsBucket });

        var options = Options.Create(_s3Settings);
        _sut = new S3FileUploadService(_s3Client, options, TimeProvider.System);
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }

    [Fact]
    public async Task MultipartUpload_LargeMP4File_CompletesSuccessfully()
    {
        var fileKey = Guid.NewGuid().ToString();
        var fileName = "large-video.mp4";
        var contentType = "video/mp4";
        int partCount = (int)Math.Ceiling((double)TotalSizeMb / PartSizeMb);

        var uploadId = await _sut.InitMultiPartUpload(
            fileKey, contentType, new Dictionary<string, string>()
            {
                ["file-name"] = fileName,
            }, CancellationToken.None);

        var parts = new List<PartETag>();
        for (int partNumber = 1; partNumber <= partCount; partNumber++)
        {
            var url = await _sut.GeneratePresignedUrlForPart(
                uploadId, partNumber, fileKey, UrlExpirationMinutes, CancellationToken.None);

            int currentPartSize = (partNumber == partCount && TotalSize % PartSize != 0)
                ? TotalSize % PartSize
                : PartSize;

            var etag = await UploadGeneratedPartData(url, currentPartSize, partNumber);
            parts.Add(new PartETag(partNumber, etag));
        }

        await _sut.CompleteMultipartUpload(
            uploadId, parts, fileKey, CancellationToken.None);

        var headObjectResponse = await _s3Client.GetObjectMetadataAsync(
            new GetObjectMetadataRequest
            {
                BucketName = _s3Settings.UploadsBucket,
                Key = fileKey
            });

        headObjectResponse.Should().NotBeNull();
        headObjectResponse.Headers.ContentType.Should().Be(contentType);
        headObjectResponse.Metadata["file-name"].Should().Be(fileName);
        headObjectResponse.ContentLength.Should().BeGreaterThan(300 * 1024 * 1024)
            .And.BeLessThanOrEqualTo(TotalSize);
    }

    private async Task<string> UploadGeneratedPartData(string presignedUrl, int partSize, int partNumber)
    {
        string httpUrl = presignedUrl.StartsWith("https://")
            ? "http://" + presignedUrl[8..]
            : presignedUrl;

        byte[] data = new byte[partSize];
        new Random(partNumber).NextBytes(data);

        var request = new HttpRequestMessage(HttpMethod.Put, httpUrl);
        request.Content = new ByteArrayContent(data);

        using var handler = new HttpClientHandler();
        handler.ServerCertificateCustomValidationCallback = (_, _, _, _) => true;

        using var httpClient = new HttpClient(handler);
        httpClient.Timeout = TimeSpan.FromMinutes(2);

        var response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        return response.Headers.GetValues("ETag").FirstOrDefault()?.Trim('"');
    }
}