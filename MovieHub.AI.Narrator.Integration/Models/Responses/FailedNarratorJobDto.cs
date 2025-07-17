namespace MovieHub.AI.Narrator.Integration.Models.Responses;

public class FailedNarratorJobDto
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