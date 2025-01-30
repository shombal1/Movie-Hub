using FluentValidation;
using MediatR;
using MovieHub.Engine.Domain.Models;

namespace MovieHub.Engine.Domain.UseCases.GetMedia;

public class GetMediaUseCase(IGetMediaStorage mediaStorage,IValidator<GetMediaQuery> validator) : IRequestHandler<GetMediaQuery, IEnumerable<Media>>
{
    public const int SizePage = 10;

    public async Task<IEnumerable<Media>> Handle(GetMediaQuery request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);
        
        int skip = (request.Page - 1) * SizePage;

        return await mediaStorage.Get(new MediaFilter(
            request.ParameterSorting, request.TypeSorting,
            request.Countries, request.MatchAllCountries,
            request.Genres, request.MatchAllGenres,
            request.Years,
            skip, SizePage), cancellationToken);
    }
}