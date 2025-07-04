namespace MovieHub.Engine.Api.Models.Responses;

public class PersonDto : BasePersonInfoDto
{
    public DateOnly? BirthDate { get; set; }
    
    public string? Biography { get; set; }
    
    public IEnumerable<Guid> MediaIds { get; set; } = new List<Guid>();
}