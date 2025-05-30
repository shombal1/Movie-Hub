using MovieHub.Engine.Domain.UseCases;

namespace MovieHub.Engine.Domain;

public interface IUnitOfWork
{
    public Task<IUnitOfWorkScope> StartScope(ReadPreference readPreference,CancellationToken cancellationToken = default);
}

public interface IUnitOfWorkScope : IAsyncDisposable
{
    public TStorage GetStorage<TStorage>() where TStorage : IStorage;
    public Task Commit(CancellationToken cancellationToken);
}