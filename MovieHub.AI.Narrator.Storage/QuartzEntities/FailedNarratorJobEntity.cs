using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MovieHub.AI.Narrator.Storage.QuartzEntities;

[Table("failed_narrator_jobs", Schema = "quartz")]
[Index(nameof(MediaId))]
public class FailedNarratorJobEntity
{
    [Key]
    public Guid Id { get; set; }

    [MaxLength(500)]
    public string JobName  { get; set; } = "";
    [MaxLength(500)]
    public string JobGroup   { get; set; } = "";
    
    public Guid MediaId { get; set; }
    
    [MaxLength(1000)]
    public string? S3Key { get; set; } = "";
    
    [MaxLength(1000)]
    public string ErrorMessage { get; set; } = "";
    
    public DateTimeOffset FailedAt { get; set; }
    
    public bool IsRetried { get; set; }
    
}