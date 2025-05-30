using System.Text.Json;
using FluentAssertions;
using MovieHub.Engine.Domain.UseCases.GetMedia;

namespace MovieHub.Engine.Domain.Tests.GetMediaTests;

public record TestGetMediaQuery(
    int Page,
    ParameterSorting ParameterSorting,
    TypeSorting TypeSorting,
    IEnumerable<string> Countries,
    bool MatchAllCountries,
    IEnumerable<string> Genres,
    bool MatchAllGenres,
    IEnumerable<int> Years) : GetMediaQuery(Page, ParameterSorting, TypeSorting, Countries,
    MatchAllCountries, Genres, MatchAllGenres, Years)
{
    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}

public class GetMediaQueryValidatorShould
{
    private readonly GetMediaQueryValidator _sut = new GetMediaQueryValidator();

    public static IEnumerable<object[]> GetInvalidRequest()
    {
        var valid = new TestGetMediaQuery(
            1,
            ParameterSorting.Alphabetically, TypeSorting.Ascending,
            [], false,
            [], false,
            []);

        yield return [valid with { Page = 0 }];
        yield return [valid with { TypeSorting = (TypeSorting)(-88) }];
        yield return [valid with { Countries = null }];
        yield return [valid with { Countries = new[] { "" } }];
        yield return [valid with { Countries = ["France", ""] }];
        yield return [valid with { Countries = new[] { " " } }];
        yield return [valid with { Countries = ["France", " "] }];
        yield return [valid with { Genres = null }];
        yield return [valid with { Genres = new[] { "" } }];
        yield return [valid with { Genres = ["Horror", ""] }];
        yield return [valid with { Genres = new[] { " " } }];
        yield return [valid with { Genres = ["Horror", " "] }];
        yield return [valid with { Years = null }];
        yield return [valid with { Years = new[] { 1100 } }];
        yield return [valid with { Years = new[] { 9999 } }];
    }

    [Theory]
    [MemberData(nameof(GetInvalidRequest))]
    public async Task ReturnFailure_WhenRequestIsValid(TestGetMediaQuery query)
    {
        var actual = await _sut.ValidateAsync(query);

        actual.IsValid.Should().BeFalse();
    }


    public static IEnumerable<object[]> GetValidRequest()
    {
        var valid = new TestGetMediaQuery(
            1,
            ParameterSorting.Alphabetically, TypeSorting.Ascending,
            [], false,
            [], false,
            []);

        yield return [valid];
        yield return [valid with { Page = 4 }];
        yield return [valid with { Countries = ["France"] }];
        yield return [valid with { Genres = ["Horror"] }];
        yield return [valid with { Years = [2010] }];
    }

    [Theory]
    [MemberData(nameof(GetValidRequest))]
    public async Task ReturnFailure_WhenRequestIsInvalid(TestGetMediaQuery query)
    {
        var actual = await _sut.ValidateAsync(query);

        actual.IsValid.Should().BeTrue();
    }
}