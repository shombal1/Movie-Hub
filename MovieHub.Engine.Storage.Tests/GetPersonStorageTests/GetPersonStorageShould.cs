using FluentAssertions;
using MovieHub.Engine.Domain.Enums;
using MovieHub.Engine.Storage.Storages;

namespace MovieHub.Engine.Storage.Tests.GetPersonStorageTests;

public class GetPersonStorageShould(GetPersonStorageTestFixture fixture) : IClassFixture<GetPersonStorageTestFixture>
{
    private readonly GetPersonStorage _sut =
        new GetPersonStorage(fixture.GetMovieHubDbContext(), fixture.GetMapper());

    [Fact]
    public async Task ReturnPersonWithAllProperties_WhenPersonExists()
    {
        var personId = Guid.Parse("819FA47A-8F47-4CD2-8CB3-1A45537BADE1");

        var actual = await _sut.Get(personId, CancellationToken.None);

        actual.Should().NotBeNull();
        actual.Id.Should().Be(personId);
        actual.FullName.Should().Be("Leonardo DiCaprio");
        actual.PhotoUrl.Should().Be("https://example.com/dicaprio.jpg");
        actual.BirthDate.Should().Be(new DateOnly(1974, 11, 11));
        actual.Biography.Should().Be("American actor and film producer");
        actual.Professions.Should().Contain(ProfessionType.Actor);
        actual.MediaIds.Should().NotBeEmpty();
    }

    [Fact]
    public async Task ReturnBasePersonInfoWithCorrectProperties_ForSpecificIds()
    {
        Guid[] personIds =
        [
            Guid.Parse("819FA47A-8F47-4CD2-8CB3-1A45537BADE1"),
            Guid.Parse("1D4224A8-BD23-4D0B-8FB4-D17D3E2C7364")
        ];

        var actual = (await _sut.Get(personIds, CancellationToken.None)).ToArray();

        actual.Should().HaveCount(2);
        actual.Should().OnlyContain(p => personIds.Contains(p.Id));
        actual.Should().OnlyContain(p => !string.IsNullOrEmpty(p.FullName));
        actual.Should().OnlyContain(p => !string.IsNullOrEmpty(p.PhotoUrl));
    }

    [Fact]
    public async Task ReturnEmptyCollection_WhenEmptyIdArrayProvided()
    {
        Guid[] emptyIds = [];

        var actual = (await _sut.Get(emptyIds, CancellationToken.None)).ToArray();

        actual.Should().BeEmpty();
    }

    [Fact]
    public async Task ReturnCorrectNumberOfPersons_WhenSkipAndTakeProvided()
    {
        var actual = (await _sut.Get(1, 2, CancellationToken.None)).ToArray();

        actual.Should().HaveCount(2);
        actual.Should().OnlyContain(p => p.Id != Guid.Empty);
        actual.Should().OnlyContain(p => !string.IsNullOrEmpty(p.FullName));
    }

    [Fact]
    public async Task ReturnEmptyCollection_WhenSkipGreaterThanTotalCount()
    {
        var actual = (await _sut.Get(skip: 10, take: 5, CancellationToken.None)).ToArray();

        actual.Should().BeEmpty();
    }

    [Fact]
    public async Task ReturnNull_WhenPersonNotFound()
    {
        var nonExistentId = Guid.Parse("1B889E2B-79BF-4757-B389-3B02ABA8E780");

        var actual = await _sut.Get(nonExistentId, CancellationToken.None);

        actual.Should().BeNull();
    }

    [Fact]
    public async Task ReturnEmptyCollection_WhenNonExistentIdsProvided()
    {
        Guid[] nonExistentIds =
        [
            Guid.Parse("06E7F51F-9A4A-4056-8BBE-0F8395DC2CE8"),
            Guid.Parse("399DDBBB-FBD1-4F1A-8AB9-F648B11931DF")
        ];

        var actual = (await _sut.Get(nonExistentIds, CancellationToken.None)).ToArray();

        actual.Should().BeEmpty();
    }
}