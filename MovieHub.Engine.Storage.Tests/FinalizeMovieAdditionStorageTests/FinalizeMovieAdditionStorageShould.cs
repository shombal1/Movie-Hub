using FluentAssertions;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MovieHub.Engine.Storage.Entities;
using MovieHub.Engine.Storage.Storages.AddMovie;

namespace MovieHub.Engine.Storage.Tests.FinalizeMovieAdditionStorageTests;

public class FinalizeMovieAdditionStorageShould(StorageTestFixture fixture) : IClassFixture<StorageTestFixture>
{
    private readonly FinalizeMovieAdditionStorage _sut =
        new FinalizeMovieAdditionStorage(fixture.GetMovieHubDbContext());
    
    [Fact]
    public async Task ReturnSuccess_WhenStatusSet()
    {
        await using var dbContext = fixture.GetMovieHubDbContext();
        var requestId = Guid.Parse("72900F29-B9F0-4C60-AA3A-E38FEE8CFC70");

        var movieRequest = new MovieRequestEntity()
        {
            Id = requestId,
            Title = "The Shawshank Redemption",
            Description =
                "Two imprisoned men bond over a number of years, finding solace and eventual redemption through acts of common decency.",
            ReleasedAt = new DateOnly(1994, 9, 23),
            PublishedAt = new DateTimeOffset(2000, 1, 2, 3, 4, 5, 6, 7, TimeSpan.FromHours(1)),
            Countries = ["USA"],
            Genres = ["Drama"],
            Directors = ["Frank Darabont"],
            OriginalUrlKey = "movie",
            Duration = TimeSpan.FromSeconds(1),
            Actors = ["Tim Robbins", "Morgan Freeman"],
            AgeRating = "R",
            Budget = 25000000,
        };

        await dbContext.MovieRequests.InsertOneAsync(movieRequest);
        
        await _sut.Update(requestId, CancellationToken.None);

        var expectedMovieRequest = await dbContext.MovieRequests.AsQueryable().FirstAsync(x => x.Id == requestId);
        expectedMovieRequest.Status.IsFinalizeMovieAddition.Should().BeTrue();
    }
}
