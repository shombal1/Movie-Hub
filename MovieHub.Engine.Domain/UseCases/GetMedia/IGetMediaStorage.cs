using MovieHub.Engine.Domain.Models;

namespace MovieHub.Engine.Domain.UseCases.GetMedia;

public interface IGetMediaStorage : IStorage
{
    public Task<IEnumerable<Media>> Get(ParameterSorting parameterSorting, TypeSorting typeSorting, string? country,
        int? year, int skip, int take, CancellationToken cancellationToken);
}