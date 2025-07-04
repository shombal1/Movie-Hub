namespace MovieHub.Engine.Domain.Exceptions;

public class PersonNotFoundException(Guid personId) 
    : DomainException(ErrorCode.Gone, $"Person with id {personId} was not found");