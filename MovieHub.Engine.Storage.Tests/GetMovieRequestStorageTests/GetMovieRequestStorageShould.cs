using FluentAssertions;
using MovieHub.Engine.Domain.Models;
using MovieHub.Engine.Storage.Common;
using MovieHub.Engine.Storage.Entities;
using MovieHub.Engine.Storage.Storages.AddMovie;
using ProcessingStatus = MovieHub.Engine.Storage.Models.ProcessingStatus;

namespace MovieHub.Engine.Storage.Tests.GetMovieRequestStorageTests;

public class GetMovieRequestStorageShould(StorageTestFixture fixture) : IClassFixture<StorageTestFixture>
{
    private readonly GetMovieRequestStorage _sut =
        new GetMovieRequestStorage(fixture.GetMovieHubDbContext(), fixture.GetMapper());

    [Fact]
    public async Task ReturnMovieRequest_WhenExists()
    {
        await using var dbContext = fixture.GetMovieHubDbContext();
        var requestId = Guid.Parse("8461C60B-0129-4BC9-9A22-4CA83F54F0AD");


        var expectedMovieRequest = new MovieRequest()
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
            Status = new Domain.Models.ProcessingStatus()
            {
                IsFinalizeMovieAddition = true,
                IsFullyProcessed = true,
                IsQualitiesProcessed = true,
                ProcessedQualities = new Dictionary<Domain.Enums.QualityType, string>()
                {
                    [Domain.Enums.QualityType.P360] = "movie-360p.mp4",
                    [Domain.Enums.QualityType.P480] = "movie-480p.mp4",
                    [Domain.Enums.QualityType.P720] = "movie-720p.mp4",
                    [Domain.Enums.QualityType.P1080] = "movie-1080p.mp4",
                    [Domain.Enums.QualityType.K4] = "movie-4k.mp4",
                    [Domain.Enums.QualityType.K2] = "movie-2.mp4"
                },
                ProcessingErrors = ["error"]
            }
        };

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
            Status = new ProcessingStatus()
            {
                IsFinalizeMovieAddition = true,
                IsFullyProcessed = true,
                IsQualitiesProcessed = true,
                ProcessedQualities = new Dictionary<QualityType, string>()
                {
                    [QualityType.P360] = "movie-360p.mp4",
                    [QualityType.P480] = "movie-480p.mp4",
                    [QualityType.P720] = "movie-720p.mp4",
                    [QualityType.P1080] = "movie-1080p.mp4",
                    [QualityType.K4] = "movie-4k.mp4",
                    [QualityType.K2] = "movie-2.mp4"
                },
                ProcessingErrors = ["error"]
            }
        };

        await dbContext.MovieRequests.InsertOneAsync(movieRequest);

        var actual = await _sut.GetMovieRequest(requestId, CancellationToken.None);
        actual.Should().NotBeNull();
        actual.Should().BeEquivalentTo(expectedMovieRequest);
    }

    [Fact]
    public async Task ReturnNull_WhenNotFound()
    {
        var requestId = Guid.Parse("8D5E2ED8-E649-4250-9A0B-A9A1EBA6EFBA");

        var actual = await _sut.GetMovieRequest(requestId, CancellationToken.None);
        actual.Should().BeNull();
    }
}