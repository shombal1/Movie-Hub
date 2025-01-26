using MovieHub.Engine.Storage.Entities;

namespace MovieHub.Engine.Storage.Tests.GetMediaStorageTests;

public class GetMediaStorageTestFixture : StorageTestFixture
{
    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();

        await using MovieHubDbContext dbContext = GetMovieHubDbContext();

        await dbContext.Media.InsertManyAsync(
        [
            new MovieEntity
            {
                Id = Guid.Parse("52A196CD-8852-45EE-A420-CB1722983E1C"),
                Title = "Parasite",
                Description = "A poor family schemes to become employed by a wealthy family.",
                ReleasedAt = new DateOnly(2019, 5, 30),
                ReleasedYearAt = 2019,
                PublishedAt = new DateTimeOffset(new DateTime(2019, 5, 29, 18, 0, 0, DateTimeKind.Utc)),
                Countries = ["South Korea"],
                Genres = ["Thriller", "Drama"],
                Director = ["Bong Joon-ho"],
                Quality = "4K"
            },
            new MovieEntity
            {
                Id = Guid.Parse("E836C3FE-3607-4232-9AEA-3FC20BCAD612"),
                Title = "Amélie",
                Description = "Amélie is an innocent and naive girl in Paris with her own sense of justice.",
                ReleasedAt = new DateOnly(2001, 4, 25),
                ReleasedYearAt = 2001,
                PublishedAt = new DateTimeOffset(new DateTime(2002, 4, 20, 14, 0, 0, DateTimeKind.Utc)),
                Countries = ["France", "Germany"],
                Genres = ["Romance", "Comedy"],
                Director = ["Jean-Pierre Jeunet"],
                Quality = "HD"
            },
            new SeriesEntity
            {
                Id = Guid.Parse("BCEAD215-8B16-47A5-B35D-27E05936BF63"),
                Title = "Dark",
                Description = "A family saga with a supernatural twist set in a German town.",
                ReleasedAt = new DateOnly(2017, 12, 1),
                ReleasedYearAt = 2017,
                PublishedAt = new DateTimeOffset(new DateTime(2018, 11, 30, 20, 0, 0, DateTimeKind.Utc)),
                Countries = ["Germany"],
                Genres = ["Mystery", "Science Fiction"],
                Director = ["Baran bo Odar", "Jantje Friese"],
                CountSeasons = 3,
                CountEpisodes = 26
            },
            new SeriesEntity
            {
                Id = Guid.Parse("190A35B9-AEBC-4A44-8EE2-46E8C166C21B"),
                Title = "Narcos",
                Description = "The rise and fall of Pablo Escobar and the Medellín Cartel.",
                ReleasedAt = new DateOnly(2015, 8, 28),
                ReleasedYearAt = 2015,
                PublishedAt = new DateTimeOffset(new DateTime(2018, 8, 27, 18, 0, 0, DateTimeKind.Utc)),
                Countries = ["USA", "Colombia"],
                Genres = ["Crime", "Drama"],
                Director = ["Chris Brancato", "Eric Newman", "Carlo Bernard"],
                CountSeasons = 3,
                CountEpisodes = 30
            },
            new MovieEntity
            {
                Id = Guid.Parse("F3A7B8C9-D4E5-4F6A-B7C8-D9E0F1A2B3C4"),
                Title = "The Intouchables",
                Description = "A quadriplegic aristocrat hires a young man from the projects to be his caregiver.",
                ReleasedAt = new DateOnly(2011, 11, 2),
                ReleasedYearAt = 2011,
                PublishedAt = new DateTimeOffset(new DateTime(2012, 5, 25, 12, 0, 0, DateTimeKind.Utc)),
                Countries = ["France"],
                Genres = ["Drama", "Comedy"],
                Director = ["Olivier Nakache", "Éric Toledano"],
                Quality = "HD"
            },
            new SeriesEntity
            {
                Id = Guid.Parse("D5E6F7A8-B9C0-4D1E-A2F3-B4C5D6E7F8A9"),
                Title = "Breaking Bad",
                Description = "A high school chemistry teacher turned meth maker partners with a former student.",
                ReleasedAt = new DateOnly(2008, 1, 20),
                ReleasedYearAt = 2008,
                PublishedAt = new DateTimeOffset(new DateTime(2008, 1, 19, 20, 0, 0, DateTimeKind.Utc)),
                Countries = ["USA"],
                Genres = ["Drama"],
                Director = ["Vince Gilligan"],
                CountSeasons = 5,
                CountEpisodes = 62
            },
            new MovieEntity
            {
                Id = Guid.Parse("A1B2C3D4-E5F6-4A5B-B6C7-D8E9F0A1B2C3"),
                Title = "Inception",
                Description =
                    "A thief who steals corporate secrets through dream-sharing technology is given a chance to have his criminal record erased.",
                ReleasedAt = new DateOnly(2010, 7, 16),
                ReleasedYearAt = 2010,
                PublishedAt = new DateTimeOffset(new DateTime(2010, 7, 15, 18, 0, 0, DateTimeKind.Utc)),
                Countries = ["USA", "UK"],
                Genres = ["Thriller", "Science Fiction", "Drama"],
                Director = ["Christopher Nolan"],
                Quality = "4K"
            }
        ]);
    }
}