using FluentAssertions;
using MovieHub.Engine.Storage.Storages;

namespace MovieHub.Engine.Storage.Tests.GetMediaFromBasketStorageTests;

public class GetMediaFromBasketStorageShould(GetMediaFromBasketStorageTestFixture fixture)
    : IClassFixture<GetMediaFromBasketStorageTestFixture>
{
    private readonly GetMediaFromBasketStorage _sut =
        new GetMediaFromBasketStorage(fixture.GetMovieHubDbContext(), fixture.GetMapper());


    public static IEnumerable<object[]> GetUsersId()
    {
        yield return [Guid.Parse("4BE7F511-B787-4B1C-9B6B-9B983F1C9A1E"), 2];
        yield return [Guid.Parse("377CA5C9-58E4-4795-AF22-DDCEA4B9E345"), 1];
    }

    [Theory]
    [MemberData(nameof(GetUsersId))]
    public async Task ReturnMediaFromBasketUsers(Guid userId, int expectedCountMedia)
    {
        int take = 0;
        int skip = 10;

        var actual = (await _sut.Get(userId, take, skip,CancellationToken.None)).ToArray();
        
        actual.Should().NotBeEmpty();
        actual.Should().HaveCount(expectedCountMedia);
        actual.Should().BeInAscendingOrder(m => m.Title);
    }

    [Fact]
    public async Task ReturnEmptyCollection_WhenUserNotExist()
    {
        var invalidUserId = Guid.Parse("F7510093-B75F-46CB-84C6-638DA04BED09");
        int take = 0;
        int skip = 10;
        
        var actual = (await _sut.Get(invalidUserId, take, skip,CancellationToken.None)).ToArray();

        actual.Should().BeEmpty();
    }
}