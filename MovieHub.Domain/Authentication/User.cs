namespace MovieHub.Domain.Authentication;

public class User(Guid id, bool isAuthenticate) : IIdentity
{
    public Guid Id { get; } = id;
    public bool IsAuthenticate { get; } = isAuthenticate;
}