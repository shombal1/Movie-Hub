using FluentValidation;

namespace MovieHub.AI.Narrator.Domain.UseCases.RetryFailedNarratorJob;

public class RetryFailedNarratorJobCommandValidator : AbstractValidator<RetryFailedNarratorJobCommand>
{
    public RetryFailedNarratorJobCommandValidator()
    {
        RuleFor(x => x.JobId)
            .NotEmpty().WithErrorCode("Empty");
    }
}