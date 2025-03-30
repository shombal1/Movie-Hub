using System.Text.Json.Serialization;

namespace MovieHub.Engine.Api.Models.Responses;


[JsonDerivedType(typeof(SeriesDto),"series")]
public class SeriesDto : MediaDto
{
    public int CountSeasons { get; set; }
    public int CountEpisodes { get; set; }
}