using FluentValidation;
using MediatR;
using MovieHub.Engine.Domain.Exceptions;
using MovieHub.Engine.Domain.Models;

namespace MovieHub.Engine.Domain.UseCases.GetMediaFullInfo;

public class GetMediaFullInfoUseCase(
    IGetMediaFullInfoStorage getMediaFullInfoStorage,
    IValidator<GetMediaFullInfoQuery> validator) : IRequestHandler<GetMediaFullInfoQuery, MediaFullInfo>
{
    public async Task<MediaFullInfo> Handle(GetMediaFullInfoQuery request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);
        
        var mediaFullInfo = await getMediaFullInfoStorage.Get(request.MediaId,cancellationToken);
        
        if(mediaFullInfo is null)
            throw new MediaNotFoundException(request.MediaId);

        return mediaFullInfo;
    }
}