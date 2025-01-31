using FluentValidation;

namespace MovieHub.Engine.Domain.UseCases.TryAddMovieToBasket;

public class TryAddMediaToBasketCommandValidator : AbstractValidator<TryAddMediaToBasketCommand>
{
    public TryAddMediaToBasketCommandValidator()
    {
        RuleFor(x => x.MediaId)
            .NotEmpty().WithErrorCode("Empty");
    }
}