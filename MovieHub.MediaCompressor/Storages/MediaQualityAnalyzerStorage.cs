using MovieHub.MediaCompressor.Domain;
using Xabe.FFmpeg;

namespace MovieHub.MediaCompressor.Storages;

public class MediaQualityAnalyzerStorage(ILogger<MediaQualityAnalyzerStorage> logger) : IMediaQualityAnalyzerStorage
{
    public async Task<QualityType> DetermineVideoQuality(string filePath, CancellationToken cancellationToken)
    {
        try
        {
            var mediaInfo = await FFmpeg.GetMediaInfo(filePath, cancellationToken);
            var videoStream = mediaInfo.VideoStreams.FirstOrDefault()
                              ?? throw new InvalidOperationException("Video stream not found");

            return MapResolutionToQualityType(videoStream.Width, videoStream.Height);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error determining video quality");
            return QualityType.P360;
        }
    }

    public async Task<string?> CompressVideoToQuality(string localStoragePath, string inputFilePath,
        QualityType targetQuality, CancellationToken cancellationToken)
    {
        try
        {
            var mediaInfo = await FFmpeg.GetMediaInfo(inputFilePath, cancellationToken);
            var videoStream = mediaInfo.VideoStreams.FirstOrDefault()
                              ?? throw new InvalidOperationException("Video stream not found");

            var aspectRatio = (double)videoStream.Width / videoStream.Height;
            var (referenceWidth, referenceHeight, bitrate) = GetCompressionParameters(targetQuality);

            int targetWidth, targetHeight;

            if (referenceWidth / aspectRatio <= referenceHeight)
            {
                targetWidth = referenceWidth;
                targetHeight = (int)Math.Round(referenceWidth / aspectRatio);
            }
            else
            {
                targetHeight = referenceHeight;
                targetWidth = (int)Math.Round(referenceHeight * aspectRatio);
            }

            targetWidth = (targetWidth % 2 == 0) ? targetWidth : targetWidth + 1;
            targetHeight = (targetHeight % 2 == 0) ? targetHeight : targetHeight + 1;

            string outputFileName =
                $"{Path.GetFileNameWithoutExtension(inputFilePath)}_{targetQuality}{Path.GetExtension(inputFilePath)}";
            string outputFilePath = Path.Combine(localStoragePath, outputFileName);

            var conversion = FFmpeg.Conversions.New();
            var streamInfo = mediaInfo.VideoStreams.First()
                .SetSize(targetWidth, targetHeight)
                .SetBitrate(bitrate * 1000);

            conversion.AddStream(streamInfo)
                .AddStream(mediaInfo.AudioStreams)
                .SetOutput(outputFilePath)
                .SetOverwriteOutput(true);

            await conversion.Start(cancellationToken);

            return outputFilePath;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error compressing video");
            return null;
        }
    }

    private (int width, int height, int bitrate) GetCompressionParameters(QualityType quality)
    {
        return quality switch
        {
            QualityType.P360 => (640, 360, 800),
            QualityType.P480 => (854, 480, 1200),
            QualityType.P720 => (1280, 720, 2500),
            QualityType.P1080 => (1920, 1080, 5000),
            QualityType.K2 => (2560, 1440, 8000),
            QualityType.K4 => (3840, 2160, 16000),
            _ => (640, 360, 800)
        };
    }

    private QualityType MapResolutionToQualityType(int width, int height)
    {
        bool isVertical = height > width;

        int mainDimension = isVertical ? height : width;

        if (isVertical)
        {
            return mainDimension switch
            {
                <= 480 => QualityType.P360,
                <= 720 => QualityType.P480,
                <= 1080 => QualityType.P720,
                <= 1440 => QualityType.P1080,
                <= 2160 => QualityType.K2,
                _ => QualityType.K4
            };
        }

        return height switch
        {
            <= 360 => QualityType.P360,
            <= 480 => QualityType.P480,
            <= 720 => QualityType.P720,
            <= 1080 => QualityType.P1080,
            <= 1440 => QualityType.K2,
            _ => QualityType.K4
        };
    }
}