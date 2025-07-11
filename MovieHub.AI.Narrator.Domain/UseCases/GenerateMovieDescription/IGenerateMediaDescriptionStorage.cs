namespace MovieHub.AI.Narrator.Domain.UseCases.GenerateMovieDescription;

public interface IGenerateMediaDescriptionStorage
{
    public Task<string> Generate(string audioText, CancellationToken cancellationToken);
}