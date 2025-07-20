using AutoMapper;
using FluentAssertions;
using MongoDB.Driver;
using Moq;
using MovieHub.Engine.Domain.Enums;
using MovieHub.Engine.Domain.Models;
using MovieHub.Engine.Storage.Entities;
using MovieHub.Engine.Storage.Storages.AddMovie;

namespace MovieHub.Engine.Storage.Tests.CreateMovieStorageTests;

public class CreateMovieStorageShould(StorageTestFixture fixture) : IClassFixture<StorageTestFixture>
{
    private readonly MovieHubDbContext _dbContext = fixture.GetMovieHubDbContext();
    private readonly IMapper _mapper = fixture.GetMapper();
    private readonly Mock<TimeProvider> _timeProvider = new();

    [Fact]
    public async Task Create_ShouldInsertMovieAndAdditionInfo_WhenValidRequest()
    {
        var fixedTime = new DateTimeOffset(2024, 1, 15, 10, 30, 0, TimeSpan.Zero);
        _timeProvider.Setup(x => x.GetUtcNow()).Returns(fixedTime);
        var storage = new CreateMovieStorage(_dbContext, _timeProvider.Object, _mapper);
        
        var actorId = Guid.NewGuid();
        var directorId = Guid.NewGuid();

        await _dbContext.Persons.InsertManyAsync([
            new PersonEntity { Id = actorId, FullName = "Actor full name" },
            new PersonEntity { Id = directorId, FullName = "Director full name" }
        ]);

        var movieRequest = new MovieRequest
        {
            Id = Guid.NewGuid(),
            Title = "Test Movie",
            Description = "Test Description",
            ReleasedAt = new DateOnly(2023, 1, 1),
            Countries = ["USA", "UK"],
            Genres = ["Action", "Drama"],
            DirectorIds = [directorId],
            ActorIds = [actorId],
            AgeRating = "PG-13",
            Budget = 1000000,
            Status = new ProcessingStatus
            {
                ProcessedQualities = new Dictionary<QualityType, string>
                {
                    { QualityType.P720, "hd720-key" },
                    { QualityType.P1080, "hd1080-key" }
                }
            }
        };


        await storage.Create(movieRequest, CancellationToken.None);

        var insertedMovie = await _dbContext.Media
            .Find(x => x.Id == movieRequest.Id)
            .FirstOrDefaultAsync();

        insertedMovie.Should().NotBeNull();
        insertedMovie!.Title.Should().Be("Test Movie");
        insertedMovie.Description.Should().Be("Test Description");
        insertedMovie.ReleasedYearAt.Should().Be(2023);
        insertedMovie.Countries.Should().BeEquivalentTo(["USA", "UK"]);
        insertedMovie.Genres.Should().BeEquivalentTo(["Action", "Drama"]);
        insertedMovie.Views.Should().Be(0);
        insertedMovie.PublishedAt.Should().Be(fixedTime);
        
        var additionInfo = await _dbContext.AdditionMediaInfo
            .OfType<AdditionMovieInfoEntity>()
            .Find(x => x.MediaId == movieRequest.Id)
            .FirstOrDefaultAsync();

        additionInfo.Should().NotBeNull();
        additionInfo!.AgeRating.Should().Be("PG-13");
        additionInfo.Budget.Should().Be(1000000);
        additionInfo.Actors.Should().HaveCount(1);
        additionInfo.Directors.Should().HaveCount(1);
        additionInfo.AvailableQualities.Should().HaveCount(2);
    }
}