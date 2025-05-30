namespace MovieHub.Engine.Domain.DomainEvents;

public abstract class DomainEvent
{
    public DomainEventType Type { get; protected init; }
}

public class DomainEventInitiateMovieAddition : DomainEvent
{
    public Guid MovieRequestId { get; }

    public DomainEventInitiateMovieAddition(Guid movieRequestId)
    {
        MovieRequestId = movieRequestId;
        Type = DomainEventType.InitiateMovieAddition;
    }
}

public class DomainEventFinalizeMovieAddition : DomainEvent
{
    public Guid MovieRequestId { get; }
    public string Key { get; }

    public DomainEventFinalizeMovieAddition(Guid movieRequestId, string key)
    {
        MovieRequestId = movieRequestId;
        Key = key;
        Type = DomainEventType.FinalizeMovieAddition;
    }
}