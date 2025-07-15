using Microsoft.Extensions.Options;
using MovieHub.MediaCompressor.Mongo;

namespace MovieHub.MediaCompressor.Domain;

public class MovieCompressUseCase: IMovieCompressUseCase
{
    public const string KeyFormat = "movies/{0}/videos/{1}/{2}";
    private readonly IGetProcessingStatusStorage _getProcessingStatusStorage;
    private readonly ILogger<MovieCompressUseCase> _logger;
    private readonly IS3StorageService _s3StorageService;
    private readonly IMediaQualityAnalyzerStorage _mediaQualityAnalyzerStorage;
    private readonly IUpdateProcessingStatusStorage _updateProcessingStatusStorage;
    private readonly string _localStoragePath;

    public MovieCompressUseCase(    
        IGetProcessingStatusStorage getProcessingStatusStorage,
        ILogger<MovieCompressUseCase> logger,
        IS3StorageService s3StorageService,
        IOptions<DownloadSettings> downloadSettingOptions,
        IMediaQualityAnalyzerStorage mediaQualityAnalyzerStorage,
        IUpdateProcessingStatusStorage updateProcessingStatusStorage)
    {
        _getProcessingStatusStorage = getProcessingStatusStorage;
        _logger = logger;
        _s3StorageService = s3StorageService;
        _mediaQualityAnalyzerStorage = mediaQualityAnalyzerStorage;
        _updateProcessingStatusStorage = updateProcessingStatusStorage;
        _localStoragePath = downloadSettingOptions.Value.LocalStoragePath;

        CleanupTemporaryFiles();
    }
    
    private void CleanupTemporaryFiles()
    {
        foreach (var file in Directory.GetFiles(_localStoragePath))
        {
            try
            {
                File.Delete(file);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Couldn't delete temporary file {FilePath}", file);
            }
        }
    }

    public async Task Compress(string key, S3Record record, CancellationToken cancellationToken)
    {
        Guid movieRequestId = Guid.Parse(record.S3.Object.UserMetadata["X-Amz-Meta-Movie-Id"]);

        ProcessingStatus? processingStatus =
            await _getProcessingStatusStorage.GetFromMovieRequest(movieRequestId, cancellationToken);


        if (processingStatus is null)
        {
            _logger.LogError("Processing status not found for movie request ID: {MovieRequestId}", movieRequestId);
            return;
        }

        if (processingStatus.IsQualitiesProcessed)
        {
            return;
        }

        string keyFormat = record.S3.Object.UserMetadata.GetValueOrDefault("x-amz-meta-key-format", KeyFormat);

        var pathDownloadMedia = await _s3StorageService.DownloadUploadMedia(key, _localStoragePath, cancellationToken);

        QualityType qualityType =
            await _mediaQualityAnalyzerStorage.DetermineVideoQuality(pathDownloadMedia, cancellationToken);
        List<QualityType> qualitiesToProcess = GetLowerQualities(qualityType);
        qualitiesToProcess.Add(qualityType);

        Dictionary<QualityType, string> processedQualities = new();

        foreach (var quality in qualitiesToProcess)
        {
            string? compressedFilePath = await _mediaQualityAnalyzerStorage.CompressVideoToQuality(
                _localStoragePath, pathDownloadMedia, quality, cancellationToken);

            if (compressedFilePath is null)
                continue;

            var qualityKey = string.Format(keyFormat,
                movieRequestId,
                quality.ToString().ToLower(),
                Path.GetFileName(compressedFilePath));

            processedQualities[quality] = qualityKey;

            await _s3StorageService.UploadProcessedMedia(qualityKey, compressedFilePath,
                cancellationToken: cancellationToken);

            try
            {
                File.Delete(compressedFilePath);
            }
            catch
            {
                // ignored
            }
        }

        await _updateProcessingStatusStorage.Update(movieRequestId, processedQualities, cancellationToken);

        try
        {
            File.Delete(pathDownloadMedia);
        }
        catch
        {
            // ignored
        }
    }

    private List<QualityType> GetLowerQualities(QualityType originalQuality)
    {
        var result = new List<QualityType>();
        var allQualities = Enum.GetValues<QualityType>().OrderBy(q => q).ToArray();

        foreach (var quality in allQualities)
        {
            if (quality < originalQuality)
            {
                result.Add(quality);
            }
        }

        return result;
    }
}