using MediatR;
using MovieHub.Engine.Domain.Models;

namespace MovieHub.Engine.Domain.UseCases.GetMedia;

public class GetMediaUseCase(IGetMediaStorage mediaStorage) : IRequestHandler<GetMediaQuery, IEnumerable<Media>>
{
    public Task<IEnumerable<Media>> Handle(GetMediaQuery request, CancellationToken cancellationToken)
    {
        return mediaStorage.Get(request.ParameterSorting, request.TypeSorting, request.Country, request.Year,
            request.Skip, request.Take, cancellationToken);
    }
}