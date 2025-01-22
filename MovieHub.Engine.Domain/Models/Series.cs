namespace MovieHub.Engine.Domain.Models;

public class Series : Media
{
    public int CountSeasons { get; set; }
    public int CountEpisodes { get; set; }
}