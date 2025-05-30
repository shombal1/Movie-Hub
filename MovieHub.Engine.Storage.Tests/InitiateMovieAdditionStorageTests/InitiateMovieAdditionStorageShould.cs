using FluentAssertions;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Moq;
using MovieHub.Engine.Storage.Common;
using MovieHub.Engine.Storage.Models;
using MovieHub.Engine.Storage.Storages.AddMovie;

namespace MovieHub.Engine.Storage.Tests.InitiateMovieAdditionStorageTests;

public class InitiateMovieAdditionStorageShould : IClassFixture<StorageTestFixture>
{
    private readonly InitiateMovieAdditionStorage _sut;
    private readonly MovieHubDbContext _dbContext;
    private readonly Mock<IGuidFactory> _guidFactoryMock;
    private readonly Guid _expectedGuid = Guid.Parse("EC1F4FEA-79F8-47D5-BBB6-CE6F0EE8AD86");

    public InitiateMovieAdditionStorageShould(StorageTestFixture fixture)
    {
        _dbContext = fixture.GetMovieHubDbContext();
        _guidFactoryMock = new Mock<IGuidFactory>();
        _guidFactoryMock.Setup(x => x.Create()).Returns(_expectedGuid);
        _sut = new InitiateMovieAdditionStorage(_dbContext, _guidFactoryMock.Object);
    }

    [Fact]
    public async Task CreateMovieRequest_SaveAllDataCorrectly_AndReturnGeneratedId()
    {
        var title = "The Shawshank Redemption";
        var description = "Two imprisoned men bond over a number of years, finding solace and eventual redemption through acts of common decency.";
        var releasedAt = new DateOnly(1994, 9, 23);
        var publishedAt = DateTimeOffset.UtcNow;
        string[] countries = ["USA"];
        string[] genres = ["Drama", "Crime"];
        string[] directors = ["Frank Darabont"];
        string[] actors = ["Tim Robbins", "Morgan Freeman", "Bob Gunton"];
        var ageRating = "R";
        var budget = 25000000L;
        
        var result = await _sut.CreateMovieRequest(
            title,
            description,
            releasedAt,
            publishedAt,
            countries,
            genres,
            directors,
            actors,
            ageRating,
            budget,
            CancellationToken.None);
        
        result.Should().Be(_expectedGuid);
        _guidFactoryMock.Verify(x => x.Create(), Times.Once);
        
        var savedEntity = await _dbContext.MovieRequests
            .AsQueryable()
            .FirstOrDefaultAsync(x=>x.Id==_expectedGuid);

        savedEntity.Should().NotBeNull();
        savedEntity.Title.Should().Be(title);
        savedEntity.Description.Should().Be(description);
        savedEntity.ReleasedAt.Should().Be(releasedAt);
        savedEntity.PublishedAt.Should().BeCloseTo(publishedAt, TimeSpan.FromSeconds(1));
        savedEntity.Countries.Should().BeEquivalentTo(countries);
        savedEntity.Genres.Should().BeEquivalentTo(genres);
        savedEntity.Directors.Should().BeEquivalentTo(directors);
        savedEntity.Actors.Should().BeEquivalentTo(actors);
        savedEntity.AgeRating.Should().Be(ageRating);
        savedEntity.Budget.Should().Be(budget);
        savedEntity.Status.Should().BeEquivalentTo(new ProcessingStatus());
        savedEntity.OriginalUrlKey.Should().BeNull();
        savedEntity.Duration.Should().BeNull();
    }
}