using MediatR;

namespace MovieHub.Engine.Domain.UseCases.AddMedia.StartMovieUpload;

public record StartMovieUploadCommand(
    Guid RequestId,
    string ContentType,
    string FileName) : IRequest<(string key, string uploadId)>;