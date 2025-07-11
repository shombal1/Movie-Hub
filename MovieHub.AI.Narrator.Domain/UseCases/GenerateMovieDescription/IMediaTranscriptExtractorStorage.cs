using System.Runtime.CompilerServices;
using MovieHub.AI.Narrator.Domain.Models;

namespace MovieHub.AI.Narrator.Domain.UseCases.GenerateMovieDescription;

public interface IMediaTranscriptExtractorStorage
{
    public IAsyncEnumerable<SegmentMedia> ExtractMediaTranscript(string wavFilePath,
        [EnumeratorCancellation] CancellationToken cancellationToken);
}