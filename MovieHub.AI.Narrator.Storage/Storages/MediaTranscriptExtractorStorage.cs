using System.Runtime.CompilerServices;
using MovieHub.AI.Narrator.Domain.Models;
using MovieHub.AI.Narrator.Domain.UseCases.GenerateMovieDescription;

namespace MovieHub.AI.Narrator.Storage.Storages;

public class MediaTranscriptExtractorStorage(WhisperProcessorFactory factory) : IMediaTranscriptExtractorStorage
{
    public async IAsyncEnumerable<SegmentMedia> ExtractMediaTranscript(string wavFilePath,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await using var fileStream = File.OpenRead(wavFilePath);
        var whisperProcessor = await factory.Create();
        await foreach (var segment in whisperProcessor.ProcessAsync(fileStream, cancellationToken))
        {
            yield return new SegmentMedia(segment.Start, segment.End, segment.Text);
        }
    }
}