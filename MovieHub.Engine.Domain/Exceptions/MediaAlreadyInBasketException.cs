namespace MovieHub.Engine.Domain.Exceptions;

public class MediaAlreadyInBasketException(Guid mediaId)
    : DomainException(ErrorCode.Conflict, $"Media with id {mediaId} already in basket");