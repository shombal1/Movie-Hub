using System.Text.Json.Serialization;

namespace MovieHub.Engine.Api.Models.Responses;

[JsonDerivedType(typeof(MovieDto),"movie")] 
public class MovieDto: MediaDto
{
    public string Quality { get; set; } = "";
}