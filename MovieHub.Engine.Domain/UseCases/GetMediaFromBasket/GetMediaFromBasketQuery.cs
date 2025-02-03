using MediatR;
using MovieHub.Engine.Domain.Models;

namespace MovieHub.Engine.Domain.UseCases.GetMediaFromBasket;

public record GetMediaFromBasketQuery(int Page) : IRequest<IEnumerable<Media>>;