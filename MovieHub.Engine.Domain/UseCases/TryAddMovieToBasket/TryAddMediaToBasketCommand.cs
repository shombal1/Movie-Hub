using MediatR;

namespace MovieHub.Engine.Domain.UseCases.TryAddMovieToBasket;

public record TryAddMediaToBasketCommand(Guid MediaId) : IRequest<Unit>;