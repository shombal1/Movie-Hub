using MovieHub.Engine.Domain.Enums;

namespace MovieHub.Engine.Domain.Models;

public class BasePersonInfo
{
    public Guid Id { get; set; }
    
    public string FullName { get; set; } = "";
    
    public string PhotoUrl { get; set; } = null!;

    public IEnumerable<ProfessionType> Professions { get; set; } = new List<ProfessionType>(); 
}