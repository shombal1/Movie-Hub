namespace MovieHub.Engine.Api.Models.Responses;

public class BasePersonInfoDto
{
    public Guid Id { get; set; }
    
    public string FullName { get; set; } = "";
    
    public string PhotoUrl { get; set; } = null!;

    public IEnumerable<string> Professions { get; set; } = new List<string>(); 
}