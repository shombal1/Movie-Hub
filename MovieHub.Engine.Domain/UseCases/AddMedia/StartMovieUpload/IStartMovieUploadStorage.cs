
namespace MovieHub.Engine.Domain.UseCases.AddMedia.StartMovieUpload;

public interface IStartMovieUploadStorage
{
    public Task<(string key, string uploadId)> InitMultiPartUpload(Guid movieId,string fileName,string contentType,CancellationToken cancellationToken);
}