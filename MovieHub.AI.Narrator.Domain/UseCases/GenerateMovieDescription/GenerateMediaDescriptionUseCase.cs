using System.Text;
using FluentValidation;

namespace MovieHub.AI.Narrator.Domain.UseCases.GenerateMovieDescription;

public class GenerateMediaDescriptionUseCase(
    IDownloadMediaStorage downloadMediaStorage,
    IMediaTranscriptExtractorStorage mediaTranscriptExtractorStorage,
    IExtractAudioStorage extractAudioStorage,
    IGenerateMediaDescriptionStorage generateMediaDescriptionStorage,
    IValidator<GenerateMovieDescriptionCommand> validator,
    IGetGeneratedDescriptionStorage getGeneratedDescriptionStorage,
    ISetAiDescriptionStorage setAiDescriptionStorage)
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
            if (!(await getGeneratedDescriptionStorage.Exists(movieId, cancellationToken)))
            {
                pathMedia = await downloadMediaStorage.DownloadMedia(key, cancellationToken);
                audioPath = await extractAudioStorage.ExtractAudio(pathMedia, cancellationToken);

                var audiText = new StringBuilder();

                await foreach (var segment in mediaTranscriptExtractorStorage.ExtractMediaTranscript(audioPath,
                                   cancellationToken))
                {
                    audiText.AppendLine(segment.Text);
                }

                var promt = await generateMediaDescriptionStorage.Generate(audiText.ToString(), cancellationToken);

                await setAiDescriptionStorage.Set(movieId, promt, cancellationToken);
            }
        }
        finally
        {
            CleanupTempFiles(pathMedia, audioPath);
        }
    }

    private void CleanupTempFiles(string mediaPath, string audioPath)
    {
        try
        {
            File.Delete(mediaPath);
        }
        catch
        {
            // ignored
        }

        try
        {
            File.Delete(audioPath);
        }
        catch
        {
            // ignored
        }
    }
}