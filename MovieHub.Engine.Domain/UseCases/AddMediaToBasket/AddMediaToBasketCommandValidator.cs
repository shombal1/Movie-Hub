using FluentValidation;

namespace MovieHub.Engine.Domain.UseCases.AddMediaToBasket;

public class AddMediaToBasketCommandValidator : AbstractValidator<AddMediaToBasketCommand>
{
    public AddMediaToBasketCommandValidator()
    {
        RuleFor(x => x.MediaId)
            .NotEmpty().WithErrorCode("Empty");
    }
}