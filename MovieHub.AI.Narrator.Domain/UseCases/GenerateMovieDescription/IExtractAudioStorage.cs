namespace MovieHub.AI.Narrator.Domain.UseCases.GenerateMovieDescription;

public interface IExtractAudioStorage
{
    Task<string> ExtractAudio(string inputFilePath, CancellationToken cancellationToken);
}