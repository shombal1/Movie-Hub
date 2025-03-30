namespace MovieHub.Engine.Domain.Models;

public class SeriesFullInfo : MediaFullInfo
{
    public int CountSeasons { get; set; }
    public int CountEpisodes { get; set; }
    public IEnumerable<Season> Seasons { get; set; } = null!;
}