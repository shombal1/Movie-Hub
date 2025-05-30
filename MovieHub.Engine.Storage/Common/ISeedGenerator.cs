using MongoDB.Driver;
using MovieHub.Engine.Storage.Entities;
using MovieHub.Engine.Storage.Models;

namespace MovieHub.Engine.Storage.Common;

public interface ISeedGenerator
{
    public Task GenerateSeed(CancellationToken cancellationToken);
}

public class SeedGenerator(MovieHubDbContext dbContext) : ISeedGenerator
{
    public async Task GenerateSeed(CancellationToken cancellationToken)
    {
        dbContext.CurrentSession.StartTransaction();
        
        Guid[] movieIds =
        [
            Guid.Parse("6439F228-B237-4966-8DEE-8CAD25BF005C"),
            Guid.Parse("DD6E2807-CA2C-4479-BD84-ADF797DC93B1"),
        ];
        
        Guid[] seriesIds =
        [
            Guid.Parse("FAD0F137-7132-469A-B27A-677DCC42EE94"),
            Guid.Parse("7CECEF0A-6236-4143-9761-E8F37CDD6566")
        ];
        
        Guid[] mediaIds = movieIds.Concat(seriesIds).ToArray();
        
        await dbContext.Media.DeleteManyAsync(dbContext.CurrentSession,
            Builders<MediaEntity>.Filter.Where(m => mediaIds.Contains(m.Id)), cancellationToken: cancellationToken);
        
        await dbContext.AdditionMediaInfo.DeleteManyAsync(dbContext.CurrentSession,
            Builders<AdditionMediaInfoEntity>.Filter.Where(m => mediaIds.Contains(m.MediaId)),
            cancellationToken: cancellationToken);
        
        await dbContext.Media.InsertManyAsync(dbContext.CurrentSession,
        [
            new MovieEntity
            {
                Id = movieIds[0],
                Title = "Inception",
                Description =
                    "A thief who steals corporate secrets through the use of dream-sharing technology is given the inverse task of planting an idea into the mind of a C.E.O.",
                ReleasedAt = new DateOnly(2010, 7, 16),
                ReleasedYearAt = 2010,
                PublishedAt = new DateTimeOffset(2010, 7, 20, 10, 30, 0, TimeSpan.Zero),
                Countries = ["USA", "UK"],
                Genres = ["Sci-Fi", "Action", "Thriller"],
                Directors = ["Christopher Nolan"],
                Views = 5000,
                Quality = "1080p"
            },
            new MovieEntity
            {
                Id = movieIds[1],
                Title = "The Dark Knight",
                Description =
                    "When the menace known as the Joker emerges from his mysterious past, he wreaks havoc and chaos on the people of Gotham.",
                ReleasedAt = new DateOnly(2008, 7, 18),
                ReleasedYearAt = 2008,
                PublishedAt = new DateTimeOffset(2008, 7, 22, 11, 0, 4, TimeSpan.Zero),
                Countries = ["USA", "UK"],
                Genres = ["Action", "Crime", "Drama"],
                Directors = ["Christopher Nolan"],
                Views = 1000,
                Quality = "4K",
            },
            new SeriesEntity
            {
                Id = seriesIds[0],
                Title = "Breaking Bad",
                Description =
                    "A high school chemistry teacher diagnosed with inoperable lung cancer turns to manufacturing and selling methamphetamine in order to secure his family's future.",
                ReleasedAt = new DateOnly(2008, 1, 20),
                ReleasedYearAt = 2008,
                PublishedAt = new DateTimeOffset(2008, 1, 21, 21, 45, 2, TimeSpan.Zero),
                Countries = ["USA"],
                Genres = ["Crime", "Drama", "Thriller"],
                Directors = ["Vince Gilligan"],
                Views = 2000,
                CountSeasons = 5,
                CountEpisodes = 62
            },
            new SeriesEntity
            {
                Id = seriesIds[1],
                Title = "Stranger Things",
                Description =
                    "When a young boy disappears, his mother, a police chief, and his friends must confront terrifying supernatural forces in order to get him back.",
                ReleasedAt = new DateOnly(2016, 7, 15),
                ReleasedYearAt = 2016,
                PublishedAt = new DateTimeOffset(2016, 7, 26, 2, 17, 8, TimeSpan.Zero),
                Countries = ["USA"],
                Genres = ["Drama", "Fantasy", "Horror"],
                Directors = ["The Duffer Brothers"],
                Views = 10_000,
                CountSeasons = 4,
                CountEpisodes = 34
            }
        ], cancellationToken: cancellationToken);
        
        await dbContext.AdditionMediaInfo.InsertManyAsync(dbContext.CurrentSession,
        [
            new AdditionMovieInfoEntity
            {
                MediaId = movieIds[0],
                Actors = ["Leonardo DiCaprio", "Joseph Gordon-Levitt", "Elliot Page"],
                StreamingUrl = "https://example.com/inception",
                Duration = TimeSpan.FromMinutes(148),
                Budget = 160_000_000,
                AgeRating = "PG-13"
            },
            new AdditionMovieInfoEntity
            {
                MediaId = movieIds[1],
                Actors = ["Christian Bale", "Heath Ledger", "Aaron Eckhart"],
                StreamingUrl = "https://example.com/dark-knight",
                Duration = TimeSpan.FromMinutes(152),
                Budget = 185_000_000,
                AgeRating = "PG-13"
            },
            new AdditionSeriesInfoEntity
            {
                MediaId = seriesIds[0],
                Actors = ["Bryan Cranston", "Aaron Paul", "Anna Gunn"],
                Budget = 40_000_000,
                AgeRating = "TV-MA"
            },
            new AdditionSeriesInfoEntity
            {
                MediaId = seriesIds[1],
                Actors = ["Millie Bobby Brown", "Finn Wolfhard", "Winona Ryder"],
                Budget = 80_000_000,
                AgeRating = "TV-14"
            }
        ], cancellationToken: cancellationToken);
        
        Guid[] seasonIds =
        [
            Guid.Parse("CCF09BCC-8B7D-4D1D-91F9-43D338DC09BC"),
            Guid.Parse("51CEEA68-ABBD-4222-B8FC-8654444A6F18"),
            Guid.Parse("9A650A73-9E91-4162-9F9E-61C0EF6C5224"),
            Guid.Parse("474EE126-529D-48AD-A545-80DBF1B19796")
        ];
        
        await dbContext.Seasons.DeleteManyAsync(dbContext.CurrentSession,
            Builders<SeasonEntity>.Filter.Where(m => seasonIds.Contains(m.Id)), cancellationToken: cancellationToken);
        
        SeasonEntity[] breakingBadSeasons =
        [
            new SeasonEntity
            {
                Id = seasonIds[0],
                SeriesId = seriesIds[0],
                Number = 1,
                ReleaseYearAt = 2008,
                Episodes =
                [
                    new Episode { Number = 1, Title = "Pilot", StreamingUrl = "url1" },
                    new Episode { Number = 2, Title = "Cat's in the Bag...", StreamingUrl = "url2" }
                ]
            },
            new SeasonEntity
            {
                Id = seasonIds[1],
                SeriesId = seriesIds[0],
                Number = 2,
                ReleaseYearAt = 2009,
                Episodes =
                [
                    new Episode { Number = 1, Title = "Seven Thirty-Seven", StreamingUrl = "url3" },
                    new Episode { Number = 2, Title = "Grilled", StreamingUrl = "url4" }
                ]
            }
        ];
        
        SeasonEntity[] strangerThingsSeasons =
        [
            new SeasonEntity
            {
                Id = seasonIds[2],
                SeriesId = seriesIds[1],
                Number = 1,
                ReleaseYearAt = 2016,
                Episodes =
                [
                    new Episode
                    {
                        Number = 1, Title = "Chapter One: The Vanishing of Will Byers", StreamingUrl = "url5"
                    },
                    new Episode { Number = 2, Title = "Chapter Two: The Weirdo on Maple Street", StreamingUrl = "url6" }
                ]
            },
            new SeasonEntity
            {
                Id = seasonIds[3],
                SeriesId = seriesIds[1],
                Number = 2,
                ReleaseYearAt = 2017,
                Episodes =
                [
                    new Episode { Number = 1, Title = "Chapter One: MADMAX", StreamingUrl = "url7" },
                    new Episode { Number = 2, Title = "Chapter Two: Trick or Treat, Freak", StreamingUrl = "url8" }
                ]
            }
        ];
        
        await dbContext.Seasons.InsertManyAsync(dbContext.CurrentSession, strangerThingsSeasons,
            cancellationToken: cancellationToken);
        await dbContext.Seasons.InsertManyAsync(dbContext.CurrentSession, breakingBadSeasons,
            cancellationToken: cancellationToken);
        
        await dbContext.CurrentSession.CommitTransactionAsync(cancellationToken);
    }
}