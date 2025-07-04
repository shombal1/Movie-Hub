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

        Guid[] personIds =
        [
            Guid.Parse("A1B2C3D4-E5F6-7890-1234-567890ABCDEF"),
            Guid.Parse("B2C3D4E5-F6A7-8901-2345-678901BCDEF0"),
            Guid.Parse("C3D4E5F6-A7B8-9012-3456-789012CDEF01"),
            Guid.Parse("D4E5F6A7-B8C9-0123-4567-890123DEF012"),
            Guid.Parse("E5F6A7B8-C9D0-1234-5678-901234EF0123")
        ];

        await dbContext.Persons.DeleteManyAsync(dbContext.CurrentSession,
            Builders<PersonEntity>.Filter.Where(p => personIds.Contains(p.Id)), cancellationToken: cancellationToken);

        await dbContext.Persons.InsertManyAsync(dbContext.CurrentSession,
        [
            new PersonEntity
            {
                Id = personIds[0],
                FullName = "Leonardo DiCaprio",
                PhotoUrl = "https://example.com/dicaprio.jpg",
                BirthDate = new DateOnly(1974, 11, 11),
                Professions = [ProfessionType.Actor],
                Biography = "American actor and producer known for his work in biographical and period films.",
                MediaIds = [movieIds[0]]
            },
            new PersonEntity
            {
                Id = personIds[1],
                FullName = "Christopher Nolan",
                PhotoUrl = "https://example.com/nolan.jpg",
                BirthDate = new DateOnly(1970, 7, 30),
                Professions = [ProfessionType.Director],
                Biography =
                    "British-American filmmaker known for his complex narratives and innovative visual techniques.",
                MediaIds = [movieIds[0], movieIds[1]]
            },
            new PersonEntity
            {
                Id = personIds[2],
                FullName = "Bryan Cranston",
                PhotoUrl = "https://example.com/cranston.jpg",
                BirthDate = new DateOnly(1956, 3, 7),
                Professions = [ProfessionType.Actor],
                Biography = "American actor best known for his roles in Breaking Bad and Malcolm in the Middle.",
                MediaIds = [seriesIds[0]]
            },
            new PersonEntity
            {
                Id = personIds[3],
                FullName = "Millie Bobby Brown",
                PhotoUrl = "https://example.com/brown.jpg",
                BirthDate = new DateOnly(2004, 2, 19),
                Professions = [ProfessionType.Actor],
                Biography = "British actress known for her role as Eleven in Stranger Things.",
                MediaIds = [seriesIds[1]]
            },
            new PersonEntity
            {
                Id = personIds[4],
                FullName = "Heath Ledger",
                PhotoUrl = "https://example.com/ledger.jpg",
                BirthDate = new DateOnly(1979, 4, 4),
                Professions = [ProfessionType.Actor],
                Biography = "Australian actor known for his versatile performances in various film genres.",
                MediaIds = [movieIds[1]]
            }
        ], cancellationToken: cancellationToken);

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
                Actors =
                [
                    new BasePersonInfo
                    {
                        Id = personIds[0],
                        FullName = "Leonardo DiCaprio",
                        Professions = [ProfessionType.Actor]
                    }
                ],
                StreamingUrl = "https://example.com/inception",
                Duration = TimeSpan.FromMinutes(148),
                Budget = 160_000_000,
                AgeRating = "PG-13",
                Directors =
                [
                    new BasePersonInfo
                    {
                        Id = personIds[1], // Christopher Nolan
                        FullName = "Christopher Nolan",
                        Professions = [ProfessionType.Director]
                    }
                ],
            },
            new AdditionMovieInfoEntity
            {
                MediaId = movieIds[1],
                Actors =
                [
                    new BasePersonInfo
                    {
                        Id = personIds[4], // Heath Ledger
                        FullName = "Heath Ledger",
                        Professions = [ProfessionType.Actor]
                    }
                ],
                StreamingUrl = "https://example.com/dark-knight",
                Duration = TimeSpan.FromMinutes(152),
                Budget = 185_000_000,
                AgeRating = "PG-13",
                Directors =
                [
                    new BasePersonInfo
                    {
                        Id = personIds[1], // Christopher Nolan
                        FullName = "Christopher Nolan",
                        Professions = [ProfessionType.Director]
                    }
                ],
            },
            new AdditionSeriesInfoEntity
            {
                MediaId = seriesIds[0],
                Actors =
                [
                    new BasePersonInfo
                    {
                        Id = personIds[2], // Bryan Cranston
                        FullName = "Bryan Cranston",
                        Professions = [ProfessionType.Actor]
                    }
                ],
                Budget = 40_000_000,
                AgeRating = "TV-MA",
                Directors = [],
            },
            new AdditionSeriesInfoEntity
            {
                MediaId = seriesIds[1],
                Actors =
                [
                    new BasePersonInfo
                    {
                        Id = personIds[3], // Millie Bobby Brown
                        FullName = "Millie Bobby Brown",
                        Professions = [ProfessionType.Actor]
                    }
                ],
                Budget = 80_000_000,
                AgeRating = "TV-14",
                Directors = [], // Нет режиссера в PersonEntity для Stranger Things
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