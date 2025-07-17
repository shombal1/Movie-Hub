using MovieHub.AI.Narrator.Domain.Models;

namespace MovieHub.AI.Narrator.Domain.UseCases.GetFailedNarratorJobs;

public interface IGetFailedNarratorJobsUseCase
{
    public Task<IEnumerable<FailedNarratorJob>> Get(GetFailedNarratorJobsQuery request,CancellationToken cancellationToken);
}