using FluentValidation;

namespace MovieHub.Engine.Domain.UseCases.RemoveMediaFromBasket;

public class RemoveMediaFromBasketCommandValidator : AbstractValidator<RemoveMediaFromBasketCommand>
{
    public RemoveMediaFromBasketCommandValidator()
    {
        RuleFor(x => x.MediaId)
            .NotEmpty().WithErrorCode("Empty");
    }
}