namespace MovieHub.Domain.Authentication;

public interface IIdentity
{
    public Guid Id { get; }
    public bool IsAuthenticate { get; }
}