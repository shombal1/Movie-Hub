using FluentAssertions;
using MovieHub.Engine.Storage.Storages;
using StackExchange.Redis;

namespace MovieHub.Engine.Storage.Tests.PopMediaViewsStorageTests;

public class PopMediaViewsStorageShould(StorageTestFixture fixture) : IClassFixture<StorageTestFixture>
{
    private readonly PopMediaViewsStorage _sut = new PopMediaViewsStorage(fixture.GetRedisDataBase());

    [Fact]
    public async Task ReturnZero_WhenNotFoundMedia()
    {
        var invalidMediaId = Guid.Parse("E80D7B22-FFF4-45E5-8DAA-C7B9EE1C1369");

        var actual = await _sut.PopViews(invalidMediaId, CancellationToken.None);

        actual.Should().Be(0);
    }

    [Fact]
    public async Task ReturnZero_AfterGot()
    {
        long expectedViewing = 15;
        var mediaId = Guid.Parse("80817753-ADC4-41B8-9C19-04F628C1E104");
        var key = string.Format(PopMediaViewsStorage.KeyFormat, mediaId);
        var redis = fixture.GetRedisDataBase();
        await redis.StringSetAsync(key, expectedViewing);

        var actual = await _sut.PopViews(mediaId, CancellationToken.None);

        actual.Should().Be(expectedViewing);
        var valueInKeyAfterPop = (await redis.StringGetAsync(key)).ToString();
        valueInKeyAfterPop.Should().NotBeNull();
        valueInKeyAfterPop.Should().Be("0");
    }
}