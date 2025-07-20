using FluentAssertions;
using MovieHub.Engine.Storage.Entities;
using MovieHub.Engine.Storage.Models;
using MovieHub.Engine.Storage.Storages.AddMovie;

namespace MovieHub.Engine.Storage.Tests.GetMovieRequestStorageTests;

public class ExistsMovieRequestStorageShould(StorageTestFixture fixture) : IClassFixture<StorageTestFixture>
{
    private readonly GetMovieRequestStorage _sut = new GetMovieRequestStorage(fixture.GetMovieHubDbContext(),fixture.GetMapper());

    [Fact]
    public async Task ReturnTrue_WhenRequestExists()
    {
        var requestId = Guid.Parse("E7B0177C-0CDA-4CAD-99C8-A39FAD1CE17D");
        await using var dbContext = fixture.GetMovieHubDbContext();
        
        var movieRequest = new MovieRequestEntity()
        {
            Id = requestId,
            Title = "The Shawshank Redemption",
            Description =
                "Two imprisoned men bond over a number of years, finding solace and eventual redemption through acts of common decency.",
            ReleasedAt = new DateOnly(1994, 9, 23), 
            Countries = ["USA"],
            Genres = ["Drama"],
            DirectorIds = [Guid.Parse("6F12D5B9-6CC0-4A9D-A9BF-812E0ED945BB")],
            OriginalUrlKey = "movie",
            ActorIds = [
                Guid.Parse("68A14E7B-3A33-4D8F-B8E0-B54C8ADCA47B"),
                Guid.Parse("8D4B2243-508F-4A7F-A43B-4F662D62806E")
            ],
            AgeRating = "R",
            Budget = 25000000,
        };

        await dbContext.MovieRequests.InsertOneAsync(movieRequest);

        var result = await _sut.Exists(requestId, CancellationToken.None);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task ReturnFalse_WhenRequestDoesNotExist()
    {
        var nonExistentRequestId = Guid.Parse("3028CD40-0792-4865-B789-1FA8BD747948");

        var result = await _sut.Exists(nonExistentRequestId, CancellationToken.None);

        result.Should().BeFalse();
    }
}