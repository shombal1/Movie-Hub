using MovieHub.Engine.Domain.Exceptions;
using MovieHub.Engine.Domain.UseCases.AddMedia.StartMovieUpload;

namespace MovieHub.Engine.Domain.UseCases.AddMedia.FinalizeMovieAddition;

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