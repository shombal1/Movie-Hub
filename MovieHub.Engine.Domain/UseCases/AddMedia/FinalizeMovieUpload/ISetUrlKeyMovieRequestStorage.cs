namespace MovieHub.Engine.Domain.UseCases.AddMedia.FinalizeMovieUpload;

public interface ISetUrlKeyMovieRequestStorage
{
    public Task SetUrlKey(
        Guid requestId,
        string urlKey,
        CancellationToken cancellationToken);
}