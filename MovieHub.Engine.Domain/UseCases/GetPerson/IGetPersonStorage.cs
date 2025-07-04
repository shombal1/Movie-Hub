using MovieHub.Engine.Domain.Models;

namespace MovieHub.Engine.Domain.UseCases.GetPerson;

public interface IGetPersonStorage
{
    public Task<Person?> Get(Guid id, CancellationToken cancellationToken);

    public Task<IEnumerable<Person>> Get(int skip, int take, CancellationToken cancellationToken);
    
    public Task<IEnumerable<BasePersonInfo>> Get(IEnumerable<Guid> personIds, CancellationToken cancellationToken);
}