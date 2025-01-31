namespace MovieHub.Engine.Domain.UseCases.GetMedia;

public static class GetMediaStorageExtension
{
    public static async Task<bool> MediaExists(this IGetMediaStorage storage, Guid id,
        CancellationToken cancellationToken)
    {
        return (await storage.Get(id, cancellationToken)) != null;
    }
}