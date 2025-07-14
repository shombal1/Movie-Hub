using FluentValidation;

namespace MovieHub.Engine.Domain.UseCases.GetBaseInfoPersons;

public class GetBaseInfoPersonsQueryValidator : AbstractValidator<GetBaseInfoPersonsQuery>
{
    public GetBaseInfoPersonsQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1).WithErrorCode("Invalid");
    }
}