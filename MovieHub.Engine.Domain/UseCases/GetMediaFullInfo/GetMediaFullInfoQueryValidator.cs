using FluentValidation;

namespace MovieHub.Engine.Domain.UseCases.GetMediaFullInfo;

public class GetMediaFullInfoQueryValidator : AbstractValidator<GetMediaFullInfoQuery>
{
    public GetMediaFullInfoQueryValidator()
    {
        RuleFor(x=>x.MediaId)
            .NotEmpty().WithErrorCode("Empty");
    }
}