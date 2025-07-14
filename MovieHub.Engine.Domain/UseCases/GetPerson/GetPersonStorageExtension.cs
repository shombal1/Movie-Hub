using MovieHub.Engine.Domain.Exceptions;

namespace MovieHub.Engine.Domain.UseCases.GetPerson;

public static class GetPersonStorageExtension
{
    public static async Task<(bool AllExist, IReadOnlyCollection<Guid> MissingIds)> EnsureAllExist(
        this IGetPersonStorage storage, 
        ICollection<Guid> personIds, 
        CancellationToken cancellationToken)
    {
        var foundPersons = await storage.Get(personIds, cancellationToken);
        var foundIds = foundPersons.Select(p => p.Id).ToHashSet();
        
        var missingIds = personIds.Except(foundIds).ToArray();
        
        return (missingIds.Length == 0, missingIds);
    }

    public static async Task ThrowIfPersonsNotFound(this IGetPersonStorage storage, 
        ICollection<Guid> personIds, 
        CancellationToken cancellationToken)
    {
        var result = await storage.EnsureAllExist(personIds, cancellationToken);

        if (!result.AllExist)
        {
            throw new PersonsNotFoundException(result.MissingIds);
        }
    }
}