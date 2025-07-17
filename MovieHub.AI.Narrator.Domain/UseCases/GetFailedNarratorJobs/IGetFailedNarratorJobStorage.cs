using MovieHub.AI.Narrator.Domain.Models;

namespace MovieHub.AI.Narrator.Domain.UseCases.GetFailedNarratorJobs;

public interface IGetFailedNarratorJobStorage
{
    public Task<IEnumerable<FailedNarratorJob>> Get(int skip,int take, CancellationToken cancellationToken);
    
    public Task<FailedNarratorJob?> Get(Guid jobId, CancellationToken cancellationToken);
}