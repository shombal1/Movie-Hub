using System.Text;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace MovieHub.AI.Narrator.Domain.UseCases.GenerateMovieDescription;

public class GenerateMediaDescriptionUseCase(
    IDownloadMediaStorage downloadMediaStorage,
    IMediaTranscriptExtractorStorage mediaTranscriptExtractorStorage,
    IExtractAudioStorage extractAudioStorage,
    IGenerateMediaDescriptionStorage generateMediaDescriptionStorage,
    IValidator<GenerateMovieDescriptionCommand> validator,
    IGetGeneratedDescriptionStorage getGeneratedDescriptionStorage,
    ISetAiDescriptionStorage setAiDescriptionStorage,
    ILogger<GenerateMediaDescriptionUseCase> logger)
    : IGenerateMediaDescriptionUseCase
{
    public async Task GenerateMediaDescription(GenerateMovieDescriptionCommand request,
        CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        Guid movieId = request.MovieId;
        string key = request.Key;

        string pathMedia = "";
        string audioPath = "";
        
        try
        {
            if (await getGeneratedDescriptionStorage.Exists(movieId, cancellationToken))
                return;

            pathMedia = await downloadMediaStorage.DownloadMedia(key, cancellationToken);
            audioPath = await extractAudioStorage.ExtractAudio(pathMedia, cancellationToken);

            var audiText = new StringBuilder();

            await foreach (var segment in mediaTranscriptExtractorStorage.ExtractMediaTranscript(audioPath,
                               cancellationToken))
            {
                logger.LogInformation("Processing segment: {Start} - {End}. text: {Text}", segment.Start, segment.End,
                    segment.Text);
                audiText.AppendLine(segment.Text);
            }

            var promt = await generateMediaDescriptionStorage.Generate(audiText.ToString(), cancellationToken);

            await setAiDescriptionStorage.Set(movieId, promt, cancellationToken);
        }
        finally
        {
            CleanupTempFiles(pathMedia, audioPath);
        }
    }

    private void CleanupTempFiles(string mediaPath, string audioPath)
    {
        TryDeleteFile(mediaPath);
        TryDeleteFile(audioPath);
    }

    private static void TryDeleteFile(string filePath)
    {
        try
        {
            File.Delete(filePath);
        }
        catch
        {
            // ignored
        }
    }
}