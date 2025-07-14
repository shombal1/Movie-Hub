namespace MovieHub.Engine.Domain.Exceptions;

public class PersonNotFoundException(Guid personId)
    : DomainException(ErrorCode.Gone, $"Person with id {personId} was not found")
{
    public Guid PersonId { get; } = personId;
}


public class PersonsNotFoundException(IEnumerable<Guid> personIds) : DomainException(ErrorCode.Gone,
    $"Persons with ids [{string.Join(", ", personIds)}] were not found")
{
    private readonly Guid[] _personIds = personIds.ToArray();
    public IReadOnlyCollection<Guid> PersonIds => _personIds;
}