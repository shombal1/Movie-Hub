using FluentValidation;
using MediatR;
using MovieHub.Engine.Domain.UseCases.AddMedia.FinalizeMovieAddition;

namespace MovieHub.Engine.Domain.UseCases.AddMedia.StartMovieUpload;

public class StartMovieUploadUseCase(
    IStartMovieUploadStorage storage,
    IGetMovieRequestStorage getMovieRequestStorage,
    IValidator<StartMovieUploadCommand> validator)
    : IRequestHandler<StartMovieUploadCommand, (string key, string uploadId)>
{
    public async Task<(string key, string uploadId)> Handle(StartMovieUploadCommand request,
        CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);
        
        await getMovieRequestStorage.ThrowIfRequestNotFound(request.RequestId, cancellationToken);

        var result = await storage.InitMultiPartUpload(
            request.RequestId, request.FileName, request.ContentType, cancellationToken);

        return result;
    }
}