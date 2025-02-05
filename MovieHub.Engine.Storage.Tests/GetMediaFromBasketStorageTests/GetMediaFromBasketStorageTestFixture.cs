using MovieHub.Engine.Storage.Entities;

namespace MovieHub.Engine.Storage.Tests.GetMediaFromBasketStorageTests;

public class GetMediaFromBasketStorageTestFixture : StorageTestFixture
{
    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();

        await using MovieHubDbContext dbContext = GetMovieHubDbContext();

        MediaEntity[] media =
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
            }
        ];

        await dbContext.Media.InsertManyAsync(media);

        UserEntity[] users =
        [
            new UserEntity()
            {
                Id = Guid.Parse("4BE7F511-B787-4B1C-9B6B-9B983F1C9A1E")
            },
            new UserEntity()
            {
                Id = Guid.Parse("377CA5C9-58E4-4795-AF22-DDCEA4B9E345")
            }
        ];

        await dbContext.Users.InsertManyAsync(users);

        await dbContext.MediaBasket.InsertManyAsync(
        [
            new MediaBasketEntity()
            {
                Id = Guid.Parse("40C54952-CDC6-4181-8E63-831E893C1697"),
                UserId = users[0].Id,
                MediaId = media[0].Id
            },
            new MediaBasketEntity()
            {
                Id = Guid.Parse("C1660047-725E-4638-8A15-F382357E36D8"),
                UserId = users[0].Id,
                MediaId = media[2].Id
            },
            
            new MediaBasketEntity()
            {
                Id = Guid.Parse("F0B2CA49-EEB8-43AF-8E57-6C196AD09519"),
                UserId = users[1].Id,
                MediaId = media[1].Id
            }
        ]);
    }
}