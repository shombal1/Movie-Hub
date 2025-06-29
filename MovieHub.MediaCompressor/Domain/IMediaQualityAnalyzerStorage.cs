namespace MovieHub.MediaCompressor.Domain;

public interface IMediaQualityAnalyzerStorage
{
    public Task<QualityType> DetermineVideoQuality(string filePath, CancellationToken cancellationToken);

    public Task<string?> CompressVideoToQuality(string localStoragePath, string inputFilePath,
        QualityType targetQuality,
        CancellationToken cancellationToken);
}