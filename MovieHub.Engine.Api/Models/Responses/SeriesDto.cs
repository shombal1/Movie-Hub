namespace MovieHub.Engine.Api.Models.Responses;

public class SeriesDto : MediaDto
{
    public override string Type { get; } = "Series";
    public int CountSeasons { get; set; }
    public int CountEpisodes { get; set; }
}