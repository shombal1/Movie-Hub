using MovieHub.Engine.Domain.Exceptions;

namespace MovieHub.Engine.Domain.UseCases.AddMedia.StartMovieUpload;

public static class IGetMovieRequestStorageExtensions
{
    public static async Task ThrowIfRequestNotFound(
        this IGetMovieRequestStorage existsMovieRequestStorage,
        Guid requestId,
        CancellationToken cancellationToken)
    {
        var exists = await existsMovieRequestStorage.Exists(requestId, cancellationToken);
        
        if (!exists)
        {
            throw new MediaRequestNotFoundException(requestId);
        } 
    }
}