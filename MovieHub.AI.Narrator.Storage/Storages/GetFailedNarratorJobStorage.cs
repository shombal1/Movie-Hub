using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MovieHub.AI.Narrator.Domain.Models;
using MovieHub.AI.Narrator.Domain.UseCases.GetFailedNarratorJobs;

namespace MovieHub.AI.Narrator.Storage.Storages;

public class GetFailedNarratorJobStorage(QuartzDbContext dbContext, IMapper mapper) : IGetFailedNarratorJobStorage
{
    public async Task<IEnumerable<FailedNarratorJob>> Get(int skip, int take, CancellationToken cancellationToken)
    {
        return await dbContext.FailedNarratorJobs
            .OrderByDescending(x=>x.FailedAt)
            .Skip(skip)
            .Take(take)
            .ProjectTo<FailedNarratorJob>(mapper.ConfigurationProvider)
            .ToArrayAsync(cancellationToken: cancellationToken);
    }

    public async Task<FailedNarratorJob?> Get(Guid jobId, CancellationToken cancellationToken)
    {
        return await dbContext.FailedNarratorJobs
            .Where(j => j.Id == jobId)
            .ProjectTo<FailedNarratorJob>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
    }
}