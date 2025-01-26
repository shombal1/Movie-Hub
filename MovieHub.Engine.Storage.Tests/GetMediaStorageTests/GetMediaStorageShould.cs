using System.Linq.Expressions;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using MovieHub.Engine.Domain.Models;
using MovieHub.Engine.Domain.UseCases.GetMedia;
using MovieHub.Engine.Storage.Storages;

namespace MovieHub.Engine.Storage.Tests.GetMediaStorageTests;

public class GetMediaStorageShould(GetMediaStorageTestFixture fixture) : IClassFixture<GetMediaStorageTestFixture>
{
    private readonly GetMediaStorage _sut =
        new GetMediaStorage(fixture.GetMovieHubDbContext(), fixture.GetMapper(), new NullLogger<GetMediaStorage>());

    private readonly MediaFilter _defaultRequest = new MediaFilter(ParameterSorting.Alphabetically,
        TypeSorting.Ascending, [], false, [], false, [], 0, 100);

    public static IEnumerable<object[]> GetYearsRequest()
    {
        yield return [new[] { 2010 }];
        yield return [new[] { 2015 }];
        yield return [new[] { 2015, 2010 }];
    }

    [Theory]
    [MemberData(nameof(GetYearsRequest))]
    public async Task ReturnMedia_FilterByYear(int[] neededYears)
    {
        var actual = (await _sut.Get(
            _defaultRequest with { Years = neededYears }, CancellationToken.None)).ToArray();

        actual.Should().NotBeEmpty();
        actual.Should().AllSatisfy(m => neededYears.Should().Contain(m.ReleasedAt.Year));
        actual.Should().BeInAscendingOrder(m => m.Title);
    }

    public static IEnumerable<object[]> GetCountriesRequest()
    {
        yield return [new[] { "Germany" }];
        yield return [new[] { "France" }];
        yield return [new[] { "Germany", "France" }];
    }

    [Theory]
    [MemberData(nameof(GetCountriesRequest))]
    public async Task ReturnMedia_FilterByMatchAllCountries(string[] countries)
    {
        var actual = (await _sut.Get(
            _defaultRequest with { Countries = countries , MatchAllCountries = true}, CancellationToken.None)).ToArray();

        actual.Should().NotBeEmpty();
        actual.Should().AllSatisfy(m => m.Countries.Should().Contain(countries));
    }

    [Theory]
    [MemberData(nameof(GetCountriesRequest))]
    public async Task ReturnMedia_FilterByMatchAnyCountries(string[] countries)
    {
        var actual = (await _sut.Get(
            _defaultRequest with { Countries = countries }, CancellationToken.None)).ToArray();

        actual.Should().NotBeEmpty();
        actual.Should().AllSatisfy(m => m.Countries.Should().Contain(c => countries.Contains(c)));
    }

    public static IEnumerable<object[]> GetGenresRequest()
    {
        yield return [new[] { "Thriller" }];
        yield return [new[] { "Drama" }];
        yield return [new[] { "Thriller", "Drama" }];
        yield return [new[] { "Thriller", "Science Fiction", "Drama" }];
    }

    [Theory]
    [MemberData(nameof(GetGenresRequest))]
    public async Task ReturnMedia_FilterByMatchAllGenres(string[] genres)
    {
        var actual = (await _sut.Get(
            _defaultRequest with { Genres = genres , MatchAllGenres = true}, CancellationToken.None)).ToArray();

        actual.Should().NotBeEmpty();
        actual.Should().AllSatisfy(m => m.Genres.Should().Contain(genres));
    }

    [Theory]
    [MemberData(nameof(GetGenresRequest))]
    public async Task ReturnMedia_FilterByMatchAnyGenres(string[] genres)
    {
        var actual = (await _sut.Get(
            _defaultRequest with { Genres = genres }, CancellationToken.None)).ToArray();

        actual.Should().NotBeEmpty();
        actual.Should().AllSatisfy(m => m.Genres.Should().Contain(c => genres.Contains(c)));
    }

    [Fact]
    public async Task ReturnEmptyCollection_WhenNotFoundMediaByFilter()
    {
        var actual = (await _sut.Get(
            _defaultRequest with { Countries = ["country that is not in the database"], MatchAllCountries = true }, CancellationToken.None)).ToArray();

        actual.Should().NotBeNull();
    }

    public static IEnumerable<object[]> GetSortedRequestAndSortingByExpectedField()
    {
        yield return [ParameterSorting.Alphabetically, (Expression<Func<Media, object>>)((Media m) => m.Title)];
        yield return [ParameterSorting.PublicationDate, (Expression<Func<Media, object>>)((Media m) => m.PublishedAt)];
        yield return [ParameterSorting.ReleaseDate, (Expression<Func<Media, object>>)((Media m) => m.ReleasedAt)];
    }

    [Theory]
    [MemberData(nameof(GetSortedRequestAndSortingByExpectedField))]
    public async Task ReturnMediaOrderedAscendingOrder(ParameterSorting parameterSorting,
        Expression<Func<Media, object>> expectedField)
    {
        var actual = (await _sut.Get(
            _defaultRequest with { ParameterSorting = parameterSorting ,TypeSorting = TypeSorting.Ascending}, CancellationToken.None)).ToArray();


        actual.Should().NotBeEmpty();
        actual.Should().BeInAscendingOrder(expectedField);
    }
    
    [Theory]
    [MemberData(nameof(GetSortedRequestAndSortingByExpectedField))]
    public async Task ReturnMediaOrderedDescendingOrder(ParameterSorting parameterSorting,
        Expression<Func<Media, object>> expectedField)
    {
        var actual = (await _sut.Get(
            _defaultRequest with { ParameterSorting = parameterSorting ,TypeSorting = TypeSorting.Descending}, CancellationToken.None)).ToArray();


        actual.Should().NotBeEmpty();
        actual.Should().BeInDescendingOrder(expectedField);
    }
    
    // TODO Write combined test
}