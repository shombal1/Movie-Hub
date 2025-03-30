namespace MovieHub.Engine.Domain.Models;

public class Season
{
    public Guid Id { get; set; }
    
    public int Number { get; set; }
    
    public int ReleaseYearAt { get; set; }
    
    public IEnumerable<Episode> Episodes { get; set; } = null!;
}