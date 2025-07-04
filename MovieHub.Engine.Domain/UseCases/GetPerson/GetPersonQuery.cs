using MediatR;
using MovieHub.Engine.Domain.Models;

namespace MovieHub.Engine.Domain.UseCases.GetPerson;

public record GetPersonQuery(Guid PersonId) : IRequest<Person>;