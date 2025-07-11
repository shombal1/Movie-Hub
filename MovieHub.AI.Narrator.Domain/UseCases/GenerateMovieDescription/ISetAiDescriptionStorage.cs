namespace MovieHub.AI.Narrator.Domain.UseCases.GenerateMovieDescription;

public interface ISetAiDescriptionStorage
{
    public Task Set(Guid movieId, string aiDescription, CancellationToken cancellationToken);
}