using MovieHub.Engine.Domain.UseCases.AddMedia.StartMovieUpload;
using MovieHub.Engine.Storage.Common;

namespace MovieHub.Engine.Storage.Storages.AddMovie;

public class StartMovieUploadStorage(IS3FileUploadService fileUploadService) : IStartMovieUploadStorage
{
    public const string KeyFormat = "movies/{0}/videos/{1}/{2}";

    public async Task<(string key, string uploadId)> InitMultiPartUpload(Guid movieId, string fileName,
        string contentType, CancellationToken cancellationToken)
    {
        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
        string fileExtension = Path.GetExtension(fileName);

        
        string normalizedFileName = FileNameNormalizer.NormalizeForFileName(fileNameWithoutExtension) + fileExtension;
        string key = string.Format(KeyFormat, movieId, "original", normalizedFileName);

        var uploadId = await fileUploadService.InitMultiPartUpload(
            key,
            contentType,
            new Dictionary<string, string>
            {
                ["x-amz-meta-movie-id"] = movieId.ToString(),
                ["x-amz-meta-file-name"] = normalizedFileName,
                ["x-amz-meta-type"] = "movie",
                ["x-amz-meta-key-format"] = KeyFormat
            }, cancellationToken);

        return (key, uploadId);
    }
}