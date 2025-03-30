using MediatR;
using MovieHub.Engine.Domain.Models;

namespace MovieHub.Engine.Domain.UseCases.GetMediaFullInfo;

public record GetMediaFullInfoQuery(Guid MediaId) : IRequest<MediaFullInfo>;