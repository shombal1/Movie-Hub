using FluentValidation;
using MovieHub.AI.Narrator.Domain.Models;

namespace MovieHub.AI.Narrator.Domain.UseCases.GetFailedNarratorJobs;

public class GetFailedNarratorJobsUseCase(
    IGetFailedNarratorJobStorage storage,
    IValidator<GetFailedNarratorJobsQuery> validator)
    : IGetFailedNarratorJobsUseCase
{
    public const int SizePage = 10;

    public async Task<IEnumerable<FailedNarratorJob>> Get(GetFailedNarratorJobsQuery request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        int skip = (request.Page - 1) * SizePage;

        return await storage.Get(skip, SizePage, cancellationToken);
    }
}