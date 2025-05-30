using MediatR;

namespace MovieHub.Engine.Domain.UseCases.AddMedia.GetMoviePartUploadUrl;

public record GetMoviePartUploadUrlQuery(string UploadId, int PartNumber, string Key): IRequest<string>;