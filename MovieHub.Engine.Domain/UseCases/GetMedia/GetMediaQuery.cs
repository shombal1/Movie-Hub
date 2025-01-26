using MediatR;
using MovieHub.Engine.Domain.Models;

namespace MovieHub.Engine.Domain.UseCases.GetMedia;

public record GetMediaQuery(
    int Page,
    ParameterSorting ParameterSorting,
    TypeSorting TypeSorting,
    IEnumerable<string> Countries,
    bool MatchAllCountries,
    IEnumerable<string> Genres,
    bool MatchAllGenres,
    IEnumerable<int> Years ): IRequest<IEnumerable<Media>>;