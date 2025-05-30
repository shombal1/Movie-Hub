using FluentValidation;

namespace MovieHub.Engine.Domain.UseCases.AddMedia.FinalizeMovieAddition;

public class FinalizeMovieAdditionCommandValidator : AbstractValidator<FinalizeMovieAdditionCommand>
{
    public FinalizeMovieAdditionCommandValidator()
    {
        RuleFor(x => x.RequestId)
            .NotEmpty().WithErrorCode("Empty");

    }
}