namespace MovieHub.Engine.Storage.Common;

public interface IGuidFactory
{
    public Guid Create();
}

public class GuidFactory : IGuidFactory
{
    public Guid Create() => Guid.NewGuid();
}