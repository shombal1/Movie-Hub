using MediatR;

namespace MovieHub.Engine.Domain.UseCases.AddMedia.PublishMovieRequest;

public record PublishMovieRequestCommand(Guid MovieRequestId) : IRequest<Unit>;