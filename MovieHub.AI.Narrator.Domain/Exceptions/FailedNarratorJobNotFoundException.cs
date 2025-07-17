namespace MovieHub.AI.Narrator.Domain.Exceptions;

public class FailedNarratorJobNotFoundException(Guid jonId) 
    : DomainException(ErrorCode.Gone, $"Failed narrator job with id {jonId} was not found");