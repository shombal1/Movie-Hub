namespace MovieHub.Engine.Domain.Authentication;

public class IdentityProvider : IIdentityProvider
{
    public IIdentity Current { get; set; }
}