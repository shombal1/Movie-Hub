namespace MovieHub.AI.Narrator.Domain.Models;

public class FailedNarratorJob
{
    public Guid Id { get; set; }
    
    public string JobName  { get; set; } = "";

    public string JobGroup   { get; set; } = "";
    
    public Guid MediaId { get; set; }
    
    public string? S3Key { get; set; } = "";
    
    public string ErrorMessage { get; set; } = "";
    
    public DateTimeOffset FailedAt { get; set; }
    
    public bool IsRetried { get; set; }
}