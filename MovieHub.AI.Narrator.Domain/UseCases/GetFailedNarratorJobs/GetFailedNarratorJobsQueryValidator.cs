using FluentValidation;

namespace MovieHub.AI.Narrator.Domain.UseCases.GetFailedNarratorJobs;

public class GetFailedNarratorJobsQueryValidator : AbstractValidator<GetFailedNarratorJobsQuery>
{
    public GetFailedNarratorJobsQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1).WithErrorCode("Invalid");
    }
}