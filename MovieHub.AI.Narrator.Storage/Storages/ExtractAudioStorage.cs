using Microsoft.Extensions.Options;
using MovieHub.AI.Narrator.Domain.UseCases.GenerateMovieDescription;
using Xabe.FFmpeg;

namespace MovieHub.AI.Narrator.Storage.Storages;

public class ExtractAudioStorage(IOptions<DownloadSettings> downloadSettings) : IExtractAudioStorage
{
    public async Task<string> ExtractAudio(string inputFilePath, CancellationToken cancellationToken)
    {
        var outputPath = Path.Combine(
            downloadSettings.Value.LocalStoragePath,
            $"{Path.GetFileNameWithoutExtension(inputFilePath)}_16khz.wav"
        );

        await FFmpeg.Conversions.New()
            .AddParameter($"-i \"{inputFilePath}\"")
            .AddParameter("-vn") // Remove video stream (-vn) to extract audio only
            .AddParameter("-acodec pcm_s16le") // Set audio codec to PCM 16-bit little-endian (standard for WAV format)
            .AddParameter("-ar 16000") // Set sample rate to 16000 Hz (16kHz)
            .AddParameter("-ac 1") // Convert to mono channel (single audio channel)
            .AddParameter($"\"{outputPath}\"")
            .Start(cancellationToken);

        return outputPath;
    }
}