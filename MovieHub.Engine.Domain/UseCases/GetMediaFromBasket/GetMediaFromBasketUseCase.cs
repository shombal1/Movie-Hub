using FluentValidation;
using MediatR;
using MovieHub.Engine.Domain.Authentication;
using MovieHub.Engine.Domain.Models;

namespace MovieHub.Engine.Domain.UseCases.GetMediaFromBasket;

public class GetMediaFromBasketUseCase(
    IValidator<GetMediaFromBasketQuery> validator,
    IGetMediaFromBasketStorage storage,
    IIdentityProvider identityProvider)
    : IRequestHandler<GetMediaFromBasketQuery, IEnumerable<Media>>
{
    public const int SizePage = 10;

    public async Task<IEnumerable<Media>> Handle(GetMediaFromBasketQuery request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        var currentUserId = identityProvider.Current.Id;
        int skip = (request.Page - 1) * SizePage;

        return await storage.Get(currentUserId, skip, SizePage, cancellationToken);
    }
}