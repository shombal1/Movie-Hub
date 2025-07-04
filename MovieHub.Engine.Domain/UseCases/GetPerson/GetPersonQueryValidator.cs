using FluentValidation;

namespace MovieHub.Engine.Domain.UseCases.GetPerson;

public class GetPersonQueryValidator : AbstractValidator<GetPersonQuery>
{
    public GetPersonQueryValidator()
    {
        RuleFor(x=>x.PersonId)
            .NotEmpty().WithErrorCode("Empty");
    }
}