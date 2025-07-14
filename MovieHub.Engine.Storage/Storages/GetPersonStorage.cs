using AutoMapper;
using AutoMapper.QueryableExtensions;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MovieHub.Engine.Domain.Models;
using MovieHub.Engine.Domain.UseCases.GetPerson;

namespace MovieHub.Engine.Storage.Storages;

public class GetPersonStorage(MovieHubDbContext dbContext, IMapper mapper) : IGetPersonStorage
{
    public async Task<Person?> Get(Guid id, CancellationToken cancellationToken)
    {
        return await dbContext.Persons
            .AsQueryable(dbContext.CurrentSession)
            .ProjectTo<Person>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(x=>x.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Person>> Get(int skip, int take, CancellationToken cancellationToken)
    {
        return await dbContext.Persons
            .AsQueryable(dbContext.CurrentSession)
            .Skip(skip)
            .Take(take)
            .ProjectTo<Person>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<BasePersonInfo>> GetBaseInfo(int skip, int take, CancellationToken cancellationToken)
    {
        return await dbContext.Persons
            .AsQueryable(dbContext.CurrentSession)
            .Skip(skip)
            .Take(take)
            .ProjectTo<BasePersonInfo>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<BasePersonInfo>> Get(IEnumerable<Guid> personIds, CancellationToken cancellationToken)
    {
        return await dbContext.Persons
            .AsQueryable(dbContext.CurrentSession)
            .Where(x => personIds.Contains(x.Id))
            .ProjectTo<BasePersonInfo>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}