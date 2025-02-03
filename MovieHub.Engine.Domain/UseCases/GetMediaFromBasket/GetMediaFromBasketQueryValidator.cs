using FluentValidation;

namespace MovieHub.Engine.Domain.UseCases.GetMediaFromBasket;

public class GetMediaFromBasketQueryValidator : AbstractValidator<GetMediaFromBasketQuery>
{
    public GetMediaFromBasketQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1).WithErrorCode("Invalid");
    }
}