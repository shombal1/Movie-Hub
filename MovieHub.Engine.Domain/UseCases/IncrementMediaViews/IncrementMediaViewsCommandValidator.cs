using FluentValidation;

namespace MovieHub.Engine.Domain.UseCases.IncrementMediaViews;

public class IncrementMediaViewsCommandValidator : AbstractValidator<IncrementMediaViewsCommand>
{
    public IncrementMediaViewsCommandValidator()
    {
        RuleFor(x => x)
            .NotEmpty().WithErrorCode("Empty");
    }
}