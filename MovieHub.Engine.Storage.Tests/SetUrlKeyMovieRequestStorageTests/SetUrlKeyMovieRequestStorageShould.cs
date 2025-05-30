using FluentAssertions;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MovieHub.Engine.Storage.Entities;
using MovieHub.Engine.Storage.Storages.AddMovie;

namespace MovieHub.Engine.Storage.Tests.SetUrlKeyMovieRequestStorageTests;

public class SetUrlKeyMovieRequestStorageShould(StorageTestFixture fixture) : IClassFixture<StorageTestFixture>
{
    private readonly SetUrlKeyMovieRequestStorage _sut =
        new SetUrlKeyMovieRequestStorage(fixture.GetMovieHubDbContext());

    [Fact]
    public async Task ReturnSuccess_WhenUrlKeySet()
    {
        var requestId = Guid.Parse("D8B40ADB-3166-4D84-8074-BA58658BCBE7");
        var urlKey = "movies/inception/inception.mp4";
        await using var dbContext = fixture.GetMovieHubDbContext();

        var movieRequest = new MovieRequestEntity
        {
            Id = requestId,
            Title = "Inception",
            Description =
                "A thief who steals corporate secrets through the use of dream-sharing technology is given the inverse task of planting an idea into the mind of a C.E.O.",
            ReleasedAt = new DateOnly(2010, 7, 16),
            PublishedAt = DateTimeOffset.UtcNow,
            Countries = ["USA", "UK"],
            Genres = ["Action", "Sci-Fi", "Thriller"],
            Directors = ["Christopher Nolan"],
            Actors = ["Leonardo DiCaprio", "Joseph Gordon-Levitt", "Ellen Page"],
            AgeRating = "PG-13",
            Budget = 160000000,
        };

        await dbContext.MovieRequests.InsertOneAsync(movieRequest);

        await _sut.SetUrlKey(requestId, urlKey, CancellationToken.None);

        var updatedRequest = await dbContext.MovieRequests
            .AsQueryable()
            .Where(x=>x.Id == requestId)
            .FirstOrDefaultAsync();
        
        updatedRequest.Should().NotBeNull();
        updatedRequest.OriginalUrlKey.Should().Be(urlKey);
    }
}