namespace MovieHub.AI.Narrator.Integration;

public class BackgroundJobOptions
{
    public const string SectionName = "BackgroundJobs";
    
    public int MaxConcurrentJobs { get; set; } = 5;
}