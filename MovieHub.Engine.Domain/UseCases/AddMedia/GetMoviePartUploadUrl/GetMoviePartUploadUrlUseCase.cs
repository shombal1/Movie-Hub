using FluentValidation;
using MediatR;

namespace MovieHub.Engine.Domain.UseCases.AddMedia.GetMoviePartUploadUrl;

public class GetMoviePartUploadUrlUseCase(
    IValidator<GetMoviePartUploadUrlQuery> validator,
    IGetMoviePartUploadUrlStorage storage)
    : IRequestHandler<GetMoviePartUploadUrlQuery, string>
{
    public async Task<string> Handle(GetMoviePartUploadUrlQuery request,
        CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        var url = await storage.GeneratePresignedUrlForPart(request.UploadId, request.PartNumber,
            request.Key, 30, cancellationToken);

        return url;
    }
}