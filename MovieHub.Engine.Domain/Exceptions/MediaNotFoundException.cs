namespace MovieHub.Engine.Domain.Exceptions;

public class MediaNotFoundException(Guid mediaId) 
    : DomainException(ErrorCode.Gone, $"Media with id {mediaId} was not found");