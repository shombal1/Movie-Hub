using FluentValidation;
using MediatR;
using MovieHub.Engine.Domain.Authentication;
using MovieHub.Engine.Domain.Exceptions;

namespace MovieHub.Engine.Domain.UseCases.RemoveMediaFromBasket;

public class RemoveMediaFromBasketUseCase(
    IValidator<RemoveMediaFromBasketCommand> validator,
    IIdentityProvider identityProvider,
    IRemoveMediaFromBasketStorage storage) : IRequestHandler<RemoveMediaFromBasketCommand,Unit>
{
    public async Task<Unit> Handle(RemoveMediaFromBasketCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request,cancellationToken);

        var currentUserId = identityProvider.Current.Id;

        var result = await storage.Remove(currentUserId, request.MediaId, cancellationToken);

        if (!result)
            throw new MediaNotFoundException(request.MediaId);
        
        return Unit.Value;
    }
}