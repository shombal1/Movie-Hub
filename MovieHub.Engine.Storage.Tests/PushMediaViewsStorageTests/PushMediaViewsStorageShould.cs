using FluentAssertions;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MovieHub.Engine.Storage.Entities;
using MovieHub.Engine.Storage.Storages;

namespace MovieHub.Engine.Storage.Tests.PushMediaViewsStorageTests;

public class PushMediaViewsStorageShould(StorageTestFixture fixture) : IClassFixture<StorageTestFixture>
{
    private readonly PushMediaViewsStorage _sut = new PushMediaViewsStorage(fixture.GetMovieHubDbContext());

    [Fact]
    public async Task ReturnFailure_WhenNonExistingMediaCreated()
    {
        await using var dbContext = fixture.GetMovieHubDbContext(); 
        Guid invalidMediaId = Guid.Parse("3ED57728-27C7-4C89-A1DC-0F842BE3C627");

        await _sut.Increment([(invalidMediaId, 0)], CancellationToken.None);

        var invalidMedia = await dbContext.Media.AsQueryable().Where(m => m.Id == invalidMediaId).FirstOrDefaultAsync();
        invalidMedia.Should().BeNull();
    }

    [Fact]
    public async Task ReturnSuccess_WhenIncrementViewsOneMedia()
    {
        await using var dbContext = fixture.GetMovieHubDbContext();
        int expectedViewing = 20;
        Guid mediaId  = Guid.Parse("44C98CB3-A7B0-491A-ADAB-47E836113946");
        await dbContext.Media.InsertOneAsync(new MovieEntity()
        {
            Id = mediaId,
            Views = 10,
            Countries = [],
            Directors = [],
            Genres = []
        });

        await _sut.Increment([(mediaId, 10)], CancellationToken.None);

        var media = await dbContext.Media.AsQueryable().Where(m => m.Id == mediaId).FirstOrDefaultAsync();
        media.Should().NotBeNull();
        media.Views.Should().Be(expectedViewing);
    }
    
    [Fact]
    public async Task ReturnSuccess_WhenIncrementViewsMultiMedia()
    {
        await using var dbContext = fixture.GetMovieHubDbContext();
        int[] expectedViews = [20,10];
        Guid[] mediaIds  = [Guid.Parse("3A25845D-CDA4-4949-B256-0F3D40B72102"),Guid.Parse("6C18008C-3A50-4FE1-AE12-852628D70886")];
        await dbContext.Media.InsertManyAsync([
            new MovieEntity()
            {
                Id = mediaIds[0],
                Views = 5
            },
            new MovieEntity()
            {
                Id = mediaIds[1],
                Views = 8
            }
        ]);

        await _sut.Increment([(mediaIds[0], 15),(mediaIds[1],2)], CancellationToken.None);

        var media = await dbContext.Media.AsQueryable().Where(m => mediaIds.Contains(m.Id)).ToListAsync();
        media.Should().NotBeNull();
        media.Should().HaveCount(2);
    
        media.Should().ContainSingle(m => m.Id == mediaIds[0] && m.Views == expectedViews[0]);
        media.Should().ContainSingle(m => m.Id == mediaIds[1] && m.Views == expectedViews[1]);
    }
}