namespace MovieHub.Engine.Domain.Exceptions;

public class MediaRequestNotFoundException(Guid requestId) 
    : DomainException(ErrorCode.Gone, $"Request with id {requestId} was not found");