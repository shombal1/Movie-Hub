
namespace MovieHub.Engine.Api.Models.Responses;


public class MovieDto: MediaDto
{
    public override string Type { get; } = "Movie";
    public string Quality { get; set; } = "";
}