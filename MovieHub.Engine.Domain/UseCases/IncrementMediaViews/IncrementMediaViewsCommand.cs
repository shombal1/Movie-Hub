using MediatR;

namespace MovieHub.Engine.Domain.UseCases.IncrementMediaViews;

public record IncrementMediaViewsCommand(Guid MediaId) : IRequest<Unit>;