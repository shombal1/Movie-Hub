using FluentAssertions;
using MovieHub.Engine.Domain.Models;
using MovieHub.Engine.Storage.Entities;
using MovieHub.Engine.Storage.Storages;

namespace MovieHub.Engine.Storage.Tests.GetMediaFullInfoStorageTests;

public class GetMediaFullInfoStorageShould(StorageTestFixture fixture) : IClassFixture<StorageTestFixture>
{
    private readonly GetMediaFullInfoStorage _sut = new(fixture.GetMovieHubDbContext());

    [Fact]
    public async Task ReturnNull_WhenPassedIdSeries()
    {
        var dbContext = fixture.GetMovieHubDbContext();
        Guid seriesId = Guid.Parse("C477E2FC-A220-4900-87BC-3652FD1540A2");

        await dbContext.Media.InsertOneAsync(new SeriesEntity()
        {
            Id = seriesId,
            Title = "Dark",
            Description = "A family saga with a supernatural twist set in a German town."
        });

        var actor = await _sut.Get(seriesId, CancellationToken.None);

        actor.Should().BeNull();
    }

    [Fact]
    public async Task ReturnNull_WhenMovieNotFound()
    {
        Guid invalidId = Guid.Parse("16196B18-EF8D-4DCC-BDBA-1528950222A3");

        var actor = await _sut.Get(invalidId, CancellationToken.None);

        actor.Should().BeNull();
    }

    [Fact]
    public async Task ReturnMovieFullInfo_WhenFound()
    {
        var dbContext = fixture.GetMovieHubDbContext();
        Guid id = Guid.Parse("E0B5A513-AFA1-4CE5-93D7-EBB9B86C0377");
        MovieFullInfo expected = new MovieFullInfo()
        {
            Id = id,
            Title = "Parasite",
            Description = "A poor family schemes to become employed by a wealthy family.",
            ReleasedAt = new DateOnly(2019, 5, 30),
            PublishedAt = new DateTimeOffset(new DateTime(2019, 5, 29, 18, 0, 0, DateTimeKind.Utc)),
            Countries = ["South Korea"],
            Genres = ["Thriller", "Drama"],
            Directors = ["Bong Joon-ho"],
            Quality = "4K",
            StreamingUrl = "https://test",
            Actors = ["John Doe"],
            Duration = TimeSpan.FromMinutes(132),
            Budget = 15_500_000,
            AgeRating = "R"
        };

        await dbContext.Media.InsertOneAsync(new MovieEntity()
        {
            Id = id,
            Title = "Parasite",
            Description = "A poor family schemes to become employed by a wealthy family.",
            ReleasedAt = new DateOnly(2019, 5, 30),
            ReleasedYearAt = 2019,
            PublishedAt = new DateTimeOffset(new DateTime(2019, 5, 29, 18, 0, 0, DateTimeKind.Utc)),
            Countries = ["South Korea"],
            Genres = ["Thriller", "Drama"],
            Directors = ["Bong Joon-ho"],
            Quality = "4K",
        });

        await dbContext.AdditionMediaInfo.InsertOneAsync(new AdditionMovieInfoEntity()
        {
            StreamingUrl = "https://test",
            Actors = ["John Doe"],
            MediaId = id,
            Duration = TimeSpan.FromMinutes(132), 
            Budget = 15_500_000,
            AgeRating = "R",
        });

        var actor = await _sut.Get(id, CancellationToken.None);

        actor.Should().NotBeNull();
        actor.Should().BeOfType<MovieFullInfo>();
        actor.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task ReturnNull_WhenPassedIdMovie()
    {
        var dbContext = fixture.GetMovieHubDbContext();
        Guid movieId = Guid.Parse("41F32840-9F6F-4201-9EA5-B34D3CD37626");

        await dbContext.Media.InsertOneAsync(new MovieEntity()
        {
            Id = movieId,
            Title = "Parasite",
            Description = "A poor family schemes to become employed by a wealthy family.",
            ReleasedAt = new DateOnly(2019, 5, 30),
            ReleasedYearAt = 2019,
            PublishedAt = new DateTimeOffset(new DateTime(2019, 5, 29, 18, 0, 0, DateTimeKind.Utc)),
            Countries = ["South Korea"],
            Genres = ["Thriller", "Drama"],
            Directors = ["Bong Joon-ho"],
            Quality = "4K"
        });

        var actor = await _sut.Get(movieId, CancellationToken.None);

        actor.Should().BeNull();
    }


    [Fact]
    public async Task ReturnSeriesFullInfo_WhenFound()
    {
        var dbContext = fixture.GetMovieHubDbContext();
        Guid seriesId = Guid.Parse("2E744694-B61D-47EC-A034-A3CDA2171010");

        var expected = new SeriesFullInfo
        {
            Id = seriesId,
            Title = "Breaking Bad",
            Description =
                "A high school chemistry teacher turned meth maker teams up with a former student to secure his family's future.",
            ReleasedAt = new DateOnly(2008, 1, 20),
            PublishedAt = new DateTimeOffset(new DateTime(2008, 1, 20, 0, 0, 0, DateTimeKind.Utc)),
            Countries = ["United States"],
            Genres = ["Crime", "Drama", "Thriller"],
            Directors = ["Vince Gilligan"],
            Actors = ["Bryan Cranston", "Aaron Paul", "Anna Gunn"],
            CountSeasons = 2,
            CountEpisodes = 4,
            Views = 10_000_000,
            Budget = 3_000_000,
            AgeRating = "TV-MA",
            Seasons = new List<Season>
            {
                new Season
                {
                    Id = Guid.Parse("50BDDF45-DDFD-40B5-8C7C-86ECC534073F"),
                    Number = 2,
                    ReleaseYearAt = 2009,
                    Episodes =
                    [
                        new MovieHub.Engine.Domain.Models.Episode
                        {
                            EpisodeNumber = 1,
                            Title = "Seven Thirty-Seven",
                            StreamingUrl = "breaking-bad-s2e1"
                        },
                        new MovieHub.Engine.Domain.Models.Episode
                        {
                            EpisodeNumber = 2,
                            Title = "Grilled",
                            StreamingUrl = "breaking-bad-s2e2"
                        }
                    ]
                },
                new Season
                {
                    Id = Guid.Parse("983B3636-B210-4BE0-91EA-C85BECD9305B"),
                    Number = 1,
                    ReleaseYearAt = 2008,
                    Episodes =
                    [
                        new MovieHub.Engine.Domain.Models.Episode
                        {
                            EpisodeNumber = 1,
                            Title = "Pilot",
                            StreamingUrl = "breaking-bad-s1e1"
                        },
                        new MovieHub.Engine.Domain.Models.Episode
                        {
                            EpisodeNumber = 2,
                            Title = "Cat's in the Bag...",
                            StreamingUrl = "breaking-bad-s1e2"
                        }
                    ]
                }
            }
        };

        await dbContext.Media.InsertOneAsync(new SeriesEntity()
        {
            Id = seriesId,
            Title = "Breaking Bad",
            Description =
                "A high school chemistry teacher turned meth maker teams up with a former student to secure his family's future.",
            ReleasedAt = new DateOnly(2008, 1, 20),
            ReleasedYearAt = 2008,
            PublishedAt = new DateTimeOffset(new DateTime(2008, 1, 20, 0, 0, 0, DateTimeKind.Utc)),
            Countries = ["United States"],
            Genres = ["Crime", "Drama", "Thriller"],
            Directors = ["Vince Gilligan"],
            CountSeasons = 2,
            CountEpisodes = 4,
            Views = 10_000_000,
        });

        await dbContext.AdditionMediaInfo.InsertOneAsync(new AdditionSeriesInfoEntity()
        {
            Actors = ["Bryan Cranston", "Aaron Paul", "Anna Gunn"],
            MediaId = seriesId,
            Budget = 3_000_000,
            AgeRating = "TV-MA"
        });

        await dbContext.Seasons.InsertManyAsync([
            new SeasonEntity()
            {
                Id = Guid.Parse("983B3636-B210-4BE0-91EA-C85BECD9305B"),
                ReleaseYearAt = 2008,
                SeriesId = seriesId,
                Number = 1,
                Episodes =
                [
                    new MovieHub.Engine.Storage.Models.Episode()
                    {
                        Number = 1,
                        StreamingUrl = "breaking-bad-s1e1",
                        Title = "Pilot",
                    },
                    new MovieHub.Engine.Storage.Models.Episode()
                    {
                        Number = 2,
                        StreamingUrl = "breaking-bad-s1e2",
                        Title = "Cat's in the Bag...",
                    }
                ]
            },
            new SeasonEntity()
            {
                Id = Guid.Parse("50BDDF45-DDFD-40B5-8C7C-86ECC534073F"),
                ReleaseYearAt = 2009,
                SeriesId = seriesId,
                Number = 2,
                Episodes =
                [
                    new MovieHub.Engine.Storage.Models.Episode()
                    {
                        Number = 1,
                        StreamingUrl = "breaking-bad-s2e1",
                        Title = "Seven Thirty-Seven",
                    },
                    new MovieHub.Engine.Storage.Models.Episode()
                    {
                        Number = 2,
                        StreamingUrl = "breaking-bad-s2e2",
                        Title = "Grilled",
                    }
                ]
            }
        ]);

        var actor = await _sut.Get(seriesId, CancellationToken.None);

        actor.Should().NotBeNull();
        actor.Should().BeOfType<SeriesFullInfo>();
        actor.Should().BeEquivalentTo(expected);
    }
    
    [Fact]
    public async Task ReturnNull_WhenSeriesNotFound()
    {
        Guid invalidId = Guid.Parse("84D1F891-9452-48DA-8F91-A368CB4ED338");
        
        var actor = await _sut.Get(invalidId,CancellationToken.None);
    
        actor.Should().BeNull();
    }
}