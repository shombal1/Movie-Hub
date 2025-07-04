using FluentValidation;
using MediatR;
using MovieHub.Engine.Domain.Exceptions;
using MovieHub.Engine.Domain.Models;

namespace MovieHub.Engine.Domain.UseCases.GetPerson;

public class GetPersonUseCase(
    IValidator<GetPersonQuery> validator,
    IGetPersonStorage storage
    ) : IRequestHandler<GetPersonQuery, Person>
{
    public async Task<Person> Handle(GetPersonQuery request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);
        
        var person = await storage.Get(request.PersonId, cancellationToken);

        if (person is null)
        {
            throw new PersonNotFoundException(request.PersonId);
        }

        return person;
    }
}