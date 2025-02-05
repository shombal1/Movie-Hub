using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using MovieHub.Engine.Domain.Models;
using MovieHub.Engine.Storage.Storages;

namespace MovieHub.Engine.Storage.Tests.GetMediaStorageTests;

public class GetSingleMediaStorageShould(GetMediaStorageTestFixture fixture) : IClassFixture<GetMediaStorageTestFixture>
{
    private readonly GetMediaStorage _sut =
        new GetMediaStorage(fixture.GetMovieHubDbContext(), fixture.GetMapper(), new NullLogger<GetMediaStorage>());


    [Fact]
    public async Task ReturnSuccess_WhenReturnMovie()
    {
        var movieId = Guid.Parse("E836C3FE-3607-4232-9AEA-3FC20BCAD612");

        var actual = await _sut.Get(movieId, CancellationToken.None);

        actual.Should().NotBeNull();
        actual.Id.Should().Be(movieId);
        actual.Should().BeOfType<Movie>();
    }

    [Fact]
    public async Task ReturnSuccess_WhenReturnMedia()
    {
        var mediaId = Guid.Parse("D5E6F7A8-B9C0-4D1E-A2F3-B4C5D6E7F8A9");

        var actual = await _sut.Get(mediaId, CancellationToken.None);

        actual.Should().NotBeNull();
        actual.Id.Should().Be(mediaId);
        actual.Should().BeOfType<Series>();
    }

    [Fact]
    public async Task ReturnSuccess_WhenReturnNull()
    {
        var invalidMediaId = Guid.Parse("489D1086-F714-421A-B7AA-71F55A18922B");

        var actual = await _sut.Get(invalidMediaId, CancellationToken.None);

        actual.Should().BeNull();
    }
}