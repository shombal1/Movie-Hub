using MediatR;

namespace MovieHub.Engine.Domain.UseCases.RemoveMediaFromBasket;

public record RemoveMediaFromBasketCommand(Guid MediaId) : IRequest<Unit>;