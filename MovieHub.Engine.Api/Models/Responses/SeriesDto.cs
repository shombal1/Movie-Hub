namespace MovieHub.Engine.Api.Models.Responses;

public class SeriesDto : MediaDto
{
    public int CountSeasons { get; set; }
    public int CountEpisodes { get; set; }
}