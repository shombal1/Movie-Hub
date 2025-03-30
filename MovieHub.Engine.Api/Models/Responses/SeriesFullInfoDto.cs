using System.Text.Json.Serialization;

namespace MovieHub.Engine.Api.Models.Responses;

[JsonDerivedType(typeof(SeriesFullInfoDto),"series")]
public class SeriesFullInfoDto : MediaFullInfoDto
{
    public int CountSeasons { get; set; }
    public int CountEpisodes { get; set; }
    public IEnumerable<SeasonDto> Seasons { get; set; } = null!;
}