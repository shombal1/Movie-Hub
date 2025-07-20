using FluentValidation;
using MediatR;
using MovieHub.Engine.Domain.Exceptions;
using MovieHub.Engine.Domain.UseCases.AddMedia.StartMovieUpload;
using MovieHub.Engine.Domain.UseCases.GetMedia;


namespace MovieHub.Engine.Domain.UseCases.AddMedia.PublishMovieRequest;

public class PublishMovieRequestUseCase(
    IValidator<PublishMovieRequestCommand> validator,
    IGetMovieRequestStorage getMovieRequestStorage,
    ICreateMovieStorage createMovieStorage,
    IGetMediaStorage getMediaStorage)
    : IRequestHandler<PublishMovieRequestCommand, Unit>
{
    public async Task<Unit> Handle(PublishMovieRequestCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);
        
        var movieRequestId = request.MovieRequestId;
        
        var mediaExists = await getMediaStorage.MediaExists(movieRequestId, cancellationToken);
        
        if (mediaExists)
        {
            throw new MovieAlreadyPublishedException(movieRequestId);
        }
        
        var movieRequest = await getMovieRequestStorage.GetMovieRequest(movieRequestId, cancellationToken);

        if (movieRequest is null)
        {
            throw new MediaRequestNotFoundException(movieRequestId);
        }

        if (!movieRequest.Status.IsQualitiesProcessed)
        {
            throw new QualitiesNotProcessedException(movieRequestId);
        }
        
        if (movieRequest.Status.AiDescription is null)
        {
            throw new AiDescriptionNotGeneratedException(movieRequestId);
        }
        await createMovieStorage.Create(movieRequest, cancellationToken);
        
        return Unit.Value;
    }
}