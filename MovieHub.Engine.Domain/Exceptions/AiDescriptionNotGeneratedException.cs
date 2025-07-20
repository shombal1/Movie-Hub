namespace MovieHub.Engine.Domain.Exceptions;

public class AiDescriptionNotGeneratedException(Guid movieRequestId) : DomainException(ErrorCode.BadRequest,
    $"AI description for movie request {movieRequestId} has not been generated yet")
{
    public Guid MovieRequestId { get; } = movieRequestId;
}