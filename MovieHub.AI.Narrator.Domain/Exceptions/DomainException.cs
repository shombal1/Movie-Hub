namespace MovieHub.AI.Narrator.Domain.Exceptions;

public class DomainException(ErrorCode errorCode,string message) : Exception(message)
{
    public ErrorCode ErrorCode { get; } = errorCode;
}