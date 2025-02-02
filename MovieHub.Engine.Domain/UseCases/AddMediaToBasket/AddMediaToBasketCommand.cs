using MediatR;

namespace MovieHub.Engine.Domain.UseCases.AddMediaToBasket;

public record AddMediaToBasketCommand(Guid MediaId) : IRequest<Unit>;