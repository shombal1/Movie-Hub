namespace MovieHub.Domain.Authentication;

public interface IIdentityProvider
{
    public IIdentity Current { get; set; }
}