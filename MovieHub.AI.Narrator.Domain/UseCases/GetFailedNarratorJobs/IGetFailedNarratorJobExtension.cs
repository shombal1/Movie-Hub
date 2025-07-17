using MovieHub.AI.Narrator.Domain.Exceptions;
using MovieHub.AI.Narrator.Domain.Models;

namespace MovieHub.AI.Narrator.Domain.UseCases.GetFailedNarratorJobs;

public static class IGetFailedNarratorJobExtension
{
    public static async Task ThrowIfJobNotFound(this IGetFailedNarratorJobStorage storage, Guid jobId,
        CancellationToken cancellationToken)
    {
        FailedNarratorJob? job = await storage.Get(jobId, cancellationToken);

        if (job is null)
        {
            throw new FailedNarratorJobNotFoundException(jobId);
        }
    }
}