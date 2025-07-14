using FluentValidation;
using MediatR;
using MovieHub.Engine.Domain.Models;
using MovieHub.Engine.Domain.UseCases.GetPerson;

namespace MovieHub.Engine.Domain.UseCases.GetBaseInfoPersons;

public class GetBaseInfoPersonsUseCase(
    IValidator<GetBaseInfoPersonsQuery> validator,
    IGetPersonStorage storage) : IRequestHandler<GetBaseInfoPersonsQuery, IEnumerable<BasePersonInfo>>
{
    public const int SizePage = 10;

    public async Task<IEnumerable<BasePersonInfo>> Handle(GetBaseInfoPersonsQuery request,
        CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        int skip = (request.Page - 1) * SizePage;
        
        return await storage.GetBaseInfo(skip, SizePage, cancellationToken);
    }
}