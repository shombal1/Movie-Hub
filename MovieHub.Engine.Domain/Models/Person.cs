namespace MovieHub.Engine.Domain.Models;

public class Person : BasePersonInfo
{
    public DateOnly? BirthDate { get; set; }
    
    public string? Biography { get; set; }
    
    public IEnumerable<Guid> MediaIds { get; set; } = new List<Guid>();
}