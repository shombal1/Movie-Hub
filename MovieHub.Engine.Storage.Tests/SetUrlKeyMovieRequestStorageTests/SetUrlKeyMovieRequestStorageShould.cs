using FluentAssertions;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MovieHub.Engine.Storage.Entities;
using MovieHub.Engine.Storage.Models;
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
            Title = "The Shawshank Redemption",
            Description =
                "Two imprisoned men bond over a number of years, finding solace and eventual redemption through acts of common decency.",
            ReleasedAt = new DateOnly(1994, 9, 23), 
            Countries = ["USA"],
            Genres = ["Drama"],
            DirectorIds  = [Guid.Parse("6F12D5B9-6CC0-4A9D-A9BF-812E0ED945BB")],
            OriginalUrlKey = "movie",
            Duration = TimeSpan.FromSeconds(1),
            ActorIds = [
                Guid.Parse("68A14E7B-3A33-4D8F-B8E0-B54C8ADCA47B"),
                Guid.Parse("8D4B2243-508F-4A7F-A43B-4F662D62806E")
            ],
            AgeRating = "R",
            Budget = 25000000,
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