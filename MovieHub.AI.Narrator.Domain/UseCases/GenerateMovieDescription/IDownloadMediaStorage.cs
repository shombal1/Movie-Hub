namespace MovieHub.AI.Narrator.Domain.UseCases.GenerateMovieDescription;

public interface IDownloadMediaStorage
{
    public Task<string> DownloadMedia(string key, CancellationToken cancellationToken);
}