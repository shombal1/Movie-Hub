using MediatR;

namespace MovieHub.Engine.Domain.UseCases.AddMedia.FinalizeMovieUpload;

public record FinalizeMovieUploadCommand(Guid RequestId,string UploadId, IEnumerable<FilePart> Parts, string Key) : IRequest<Unit>;