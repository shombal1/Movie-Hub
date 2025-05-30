namespace MovieHub.Engine.Domain.UseCases.AddMedia.FinalizeMovieUpload;

public interface IFinalizeMovieUploadStorage
{
    public Task FinalizeMultiPartUpload(string uploadId, IEnumerable<FilePart> parts, string key,
        CancellationToken cancellationToken);
}