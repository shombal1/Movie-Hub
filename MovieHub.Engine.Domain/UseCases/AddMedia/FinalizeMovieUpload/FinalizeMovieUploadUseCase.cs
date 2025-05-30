using FluentValidation;
using MediatR;
using MovieHub.Engine.Domain.UseCases.AddMedia.FinalizeMovieAddition;
using MovieHub.Engine.Domain.UseCases.AddMedia.StartMovieUpload;

namespace MovieHub.Engine.Domain.UseCases.AddMedia.FinalizeMovieUpload;

public class FinalizeMovieUploadUseCase(
    IValidator<FinalizeMovieUploadCommand> validator,
    IFinalizeMovieUploadStorage finalizeMovieUploadStorage,
    ISetUrlKeyMovieRequestStorage setUrlKeyMovieRequestStorage,
    IGetMovieRequestStorage getMovieRequestStorage)
    : IRequestHandler<FinalizeMovieUploadCommand, Unit>
{
    public async Task<Unit> Handle(FinalizeMovieUploadCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        await getMovieRequestStorage.ThrowIfRequestNotFound(request.RequestId, cancellationToken);

        await Task.WhenAll(
            finalizeMovieUploadStorage.FinalizeMultiPartUpload(
                request.UploadId,
                request.Parts,
                request.Key,
                cancellationToken),
            setUrlKeyMovieRequestStorage.SetUrlKey(
                request.RequestId,
                request.Key,
                cancellationToken)
        );

        return Unit.Value;
    }
}