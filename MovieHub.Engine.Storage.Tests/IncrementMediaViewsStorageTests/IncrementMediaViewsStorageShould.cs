using FluentAssertions;
using MovieHub.Engine.Storage.Storages;

namespace MovieHub.Engine.Storage.Tests.IncrementMediaViewsStorageTests;

public class IncrementMediaViewsStorageShould(StorageTestFixture fixture) : IClassFixture<StorageTestFixture>
{
    private readonly IncrementMediaViewsStorage _sut = new IncrementMediaViewsStorage(fixture.GetRedisDataBase());

    [Fact]
    public async Task CreateKey_WhenNonExits()
    {
        var database = fixture.GetRedisDataBase();
        var mediaId = Guid.Parse("4886A322-655A-4A5A-8994-D967C87C277A");
        var key = string.Format(PopMediaViewsStorage.KeyFormat, mediaId);
        
        await _sut.Increment(mediaId, CancellationToken.None);

        var value = await database.StringGetAsync(key);
        value.HasValue.Should().BeTrue();
    }
    
    [Theory]
    [InlineData(0,1)]
    [InlineData(11,12)]
    public async Task ReturnSuccess_WhenValuesIncremented(int initValue,int expectedValue)
    {
        var database = fixture.GetRedisDataBase();
        var mediaId = Guid.Parse("88119879-794C-4EB2-B2C8-8D8E1BB144BE");
        var key = string.Format(PopMediaViewsStorage.KeyFormat, mediaId);
        await database.StringSetAsync(key, initValue);
        
        await _sut.Increment(mediaId, CancellationToken.None);

        var value = await database.StringGetAsync(key);
        value.HasValue.Should().BeTrue();
        int valueKey = int.Parse(value.ToString());
        valueKey.Should().Be(expectedValue);
    }
}