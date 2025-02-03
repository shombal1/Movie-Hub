using FluentValidation;
using MediatR;
using MovieHub.Engine.Domain.Authentication;
using MovieHub.Engine.Domain.Exceptions;
using MovieHub.Engine.Domain.UseCases.GetMedia;

namespace MovieHub.Engine.Domain.UseCases.AddMediaToBasket;

public class AddMediaToBasketUseCase(
    IValidator<AddMediaToBasketCommand> validator,
    IGetMediaStorage getMediaStorage,
    ITryAddMediaToBasketStorage tryAddMediaToBasketStorage,
    IIdentityProvider identityProvider) : IRequestHandler<AddMediaToBasketCommand, Unit>
{
    public async Task<Unit> Handle(AddMediaToBasketCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);
        
        var currentUserId = identityProvider.Current.Id;

        if (!(await getMediaStorage.MediaExists(request.MediaId, cancellationToken)))
            throw new MediaNotFoundException(request.MediaId);
        
        var isMediaExists = await tryAddMediaToBasketStorage.TryAdd(currentUserId, request.MediaId, cancellationToken);

        if (!isMediaExists)
            throw new MediaAlreadyInBasketException(request.MediaId);

        return Unit.Value;
    }
}