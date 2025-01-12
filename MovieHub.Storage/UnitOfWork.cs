using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using MovieHub.Domain;
using ReadPreference = MovieHub.Domain.ReadPreference;

namespace MovieHub.Storage;

public class UnitOfWork(IServiceProvider serviceProvider) : IUnitOfWork
{
    public async Task<IUnitOfWorkScope> StartScope(
        ReadPreference readPreference,
        CancellationToken cancellationToken = default)
    { 
        var scope = serviceProvider.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MovieHubDbContext>();

        dbContext.CurrentSession.StartTransaction(new TransactionOptions(
            readPreference: new Optional<MongoDB.Driver.ReadPreference>(readPreference switch
            {
                ReadPreference.Primary => MongoDB.Driver.ReadPreference.Primary,
                ReadPreference.Secondary => MongoDB.Driver.ReadPreference.Secondary,
                ReadPreference.PrimaryPreferred => MongoDB.Driver.ReadPreference.PrimaryPreferred,
                _ => MongoDB.Driver.ReadPreference.PrimaryPreferred
            })));


        return new UnitOfWorkScope(dbContext.CurrentSession, scope);
    }
}

public class UnitOfWorkScope(IClientSessionHandle session, IServiceScope serviceScope) : IUnitOfWorkScope
{
    public async ValueTask DisposeAsync()
    {
        if (serviceScope is AsyncServiceScope asyncServiceScope)
        {
            await asyncServiceScope.DisposeAsync();
        }
        else
        {
            serviceScope.Dispose();
        }
    }

    public TStorage GetStorage<TStorage>() where TStorage : IStorage
    {
        return serviceScope.ServiceProvider.GetRequiredService<TStorage>();
    }

    public async Task Commit(CancellationToken cancellationToken)
    {
        await session.CommitTransactionAsync(cancellationToken);
    }
}