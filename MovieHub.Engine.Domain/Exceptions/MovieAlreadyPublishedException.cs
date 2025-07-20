namespace MovieHub.Engine.Domain.Exceptions;

public class MovieAlreadyPublishedException(Guid movieRequestId) 
    : DomainException(ErrorCode.BadRequest, $"Movie with request ID {movieRequestId} has already been published.")
{
    public Guid MovieRequestId { get; } = movieRequestId;
}