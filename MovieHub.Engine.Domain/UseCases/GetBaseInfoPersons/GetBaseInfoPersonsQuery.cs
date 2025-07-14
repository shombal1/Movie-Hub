using MediatR;
using MovieHub.Engine.Domain.Models;

namespace MovieHub.Engine.Domain.UseCases.GetBaseInfoPersons;

public record GetBaseInfoPersonsQuery(int Page) : IRequest<IEnumerable<BasePersonInfo>>;