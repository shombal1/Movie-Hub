namespace MovieHub.Engine.Storage;

public class S3Settings
{
    public string ProcessedBucket { get; init; } = ""; 
    public string UploadsBucket { get; init; } = ""; 
}