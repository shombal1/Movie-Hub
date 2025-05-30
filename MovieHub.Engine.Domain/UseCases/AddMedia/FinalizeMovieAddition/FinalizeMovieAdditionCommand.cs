using MediatR;

namespace MovieHub.Engine.Domain.UseCases.AddMedia.FinalizeMovieAddition;

public record FinalizeMovieAdditionCommand(Guid RequestId) : IRequest<Unit>;