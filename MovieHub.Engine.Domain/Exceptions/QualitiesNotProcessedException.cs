namespace MovieHub.Engine.Domain.Exceptions;

public class QualitiesNotProcessedException(Guid movieRequestId) : DomainException(ErrorCode.BadRequest,
    $"Qualities for movie request {movieRequestId} are not processed yet")
{
    public Guid MovieRequestId { get; } = movieRequestId;
}