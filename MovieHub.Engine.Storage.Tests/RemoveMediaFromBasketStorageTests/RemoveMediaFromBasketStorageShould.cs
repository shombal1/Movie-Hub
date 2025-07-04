using FluentAssertions;
using MongoDB.Driver;
using MovieHub.Engine.Storage.Entities;
using MovieHub.Engine.Storage.Storages;

namespace MovieHub.Engine.Storage.Tests.RemoveMediaFromBasketStorageTests;

public class RemoveMediaFromBasketStorageShould(StorageTestFixture fixture) : IClassFixture<StorageTestFixture>
{
    private readonly RemoveMediaFromBasketStorage _sut =
        new RemoveMediaFromBasketStorage(fixture.GetMovieHubDbContext());


    [Fact]
    public async Task ReturnTrue_WhenMediaFromBasketUsersHasBeenRemoved()
    {
        await using MovieHubDbContext dbContext = fixture.GetMovieHubDbContext();

        var mediaId = Guid.Parse("96277B25-0BE9-40C7-B261-18A953B1053B");
        var userId = Guid.Parse("473CBB00-477D-49A3-B6F9-9BD681F6ADB0");

        MediaEntity media = new MovieEntity
        {
            Id = mediaId,
            Title = "Parasite",
            Description = "A poor family schemes to become employed by a wealthy family.",
            ReleasedAt = new DateOnly(2019, 5, 30),
            ReleasedYearAt = 2019,
            PublishedAt = new DateTimeOffset(new DateTime(2019, 5, 29, 18, 0, 0, DateTimeKind.Utc)),
            Countries = ["South Korea"],
            Genres = ["Thriller", "Drama"],
            Quality = "4K"
        };

        await dbContext.Media.InsertOneAsync(media);

        UserEntity user = new UserEntity()
        {
            Id = userId
        };

        await dbContext.Users.InsertOneAsync(user);

        await dbContext.MediaBasket.InsertOneAsync(new MediaBasketEntity()
        {
            Id = Guid.Parse("56E58269-ED15-4F82-816E-4E68DADB9473"),
            UserId = userId,
            MediaId = mediaId
        });

        var actual = await _sut.Remove(userId, mediaId, CancellationToken.None);

        actual.Should().BeTrue();
        var allMediaBasket = await dbContext.MediaBasket.Find(FilterDefinition<MediaBasketEntity>.Empty).ToListAsync();
        allMediaBasket.Should().NotContain(m => m.UserId == userId && m.MediaId == mediaId);
    }
    
    [Fact]
    public async Task ReturnFalse_WhenMediaFromBasketUsersHasNotBeenRemoved()
    {
        await using MovieHubDbContext dbContext = fixture.GetMovieHubDbContext();
        
        var mediaId = Guid.Parse("FB55913B-7585-4B33-94D5-6B9CF126720A");
        var userId = Guid.Parse("D9414095-1C19-4C84-A9C3-184626F8FBD3");

        MediaEntity media = new MovieEntity
        {
            Id = mediaId,
            Title = "Parasite",
            Description = "A poor family schemes to become employed by a wealthy family.",
            ReleasedAt = new DateOnly(2019, 5, 30),
            ReleasedYearAt = 2019,
            PublishedAt = new DateTimeOffset(new DateTime(2019, 5, 29, 18, 0, 0, DateTimeKind.Utc)),
            Countries = ["South Korea"],
            Genres = ["Thriller", "Drama"],
            Quality = "4K"
        };

        await dbContext.Media.InsertOneAsync(media);

        UserEntity user = new UserEntity()
        {
            Id = userId
        };

        await dbContext.Users.InsertOneAsync(user);

        await dbContext.MediaBasket.InsertOneAsync(new MediaBasketEntity()
        {
            Id = Guid.Parse("B27B9FBD-F482-42BC-BBA6-D8F54347C710"),
            UserId = userId,
            MediaId = mediaId
        });

        var invalidMediaId = Guid.Parse("1149C270-0A14-4DBF-A33E-B4A2DB691C2C");
        
        var actual = await _sut.Remove(userId, invalidMediaId, CancellationToken.None);
        
        actual.Should().BeFalse();
    }
    
}