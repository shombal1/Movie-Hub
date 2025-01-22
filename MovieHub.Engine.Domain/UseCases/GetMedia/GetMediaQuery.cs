using MediatR;
using MovieHub.Engine.Domain.Models;

namespace MovieHub.Engine.Domain.UseCases.GetMedia;

public record GetMediaQuery(ParameterSorting ParameterSorting, TypeSorting TypeSorting, string? Country,
    int? Year, int Skip, int Take): IRequest<IEnumerable<Media>>;