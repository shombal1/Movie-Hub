namespace MovieHub.Engine.Domain.UseCases.AddMedia.FinalizeMovieAddition;

public interface IFinalizeMovieAdditionStorage : IStorage
{
    public Task Update(Guid requestId, CancellationToken cancellationToken);
}