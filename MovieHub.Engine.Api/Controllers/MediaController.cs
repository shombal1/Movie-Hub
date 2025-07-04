using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MovieHub.Engine.Api.Models.Requests;
using MovieHub.Engine.Api.Models.Responses;
using MovieHub.Engine.Domain.Models;
using MovieHub.Engine.Domain.UseCases.GetMedia;
using MovieHub.Engine.Domain.UseCases.GetMediaFullInfo;
using MovieHub.Engine.Domain.UseCases.IncrementMediaViews;
using ParameterSorting = MovieHub.Engine.Api.Enums.ParameterSorting;
using TypeSorting = MovieHub.Engine.Api.Enums.TypeSorting;

namespace MovieHub.Engine.Api.Controllers;

[ApiController]
[Route("api")]
public class MediaController : ControllerBase
{
    [HttpGet]
    [Route("v2/media{page}")]
    public async Task<IActionResult> GetMedia(
        [FromServices] IMediator mediator,
        [FromServices] IMapper mapper,
        [FromQuery] GetMediaDto getMediaDto,
        [FromRoute] int page,
        CancellationToken cancellationToken)
    {
        var query = new GetMediaQuery(page, getMediaDto.ParameterSorting switch
            {
                ParameterSorting.Alphabetically => Domain.UseCases.GetMedia.ParameterSorting.Alphabetically,
                ParameterSorting.PublicationDate => Domain.UseCases.GetMedia.ParameterSorting.PublicationDate,
                ParameterSorting.ReleaseDate => Domain.UseCases.GetMedia.ParameterSorting.ReleaseDate,
                _ => throw new ArgumentOutOfRangeException()
            },
            getMediaDto.TypeSorting switch
            {
                TypeSorting.Ascending => Domain.UseCases.GetMedia.TypeSorting.Ascending,
                TypeSorting.Descending => Domain.UseCases.GetMedia.TypeSorting.Descending,
                _ => throw new ArgumentOutOfRangeException()
            },
            getMediaDto.Countries, getMediaDto.MatchAllCountries,
            getMediaDto.Genres, getMediaDto.MatchAllGenres,
            getMediaDto.Years);

        var result = await mediator.Send(query, cancellationToken);

        return Ok(mapper.Map<IEnumerable<MediaDto>>(result));
    }

    [HttpGet]
    [Route("v2/media/{mediaId}/full-info")]
    public async Task<IActionResult> GetMediaFullInfo(
        [FromRoute] Guid mediaId, 
        [FromServices] IMapper mapper,
        [FromServices] IMediator mediator)
    {
        MediaFullInfo mediaFullInfo = await mediator.Send(new GetMediaFullInfoQuery(mediaId));
        
        return Ok(mapper.Map<MediaFullInfoDto>(mediaFullInfo));
    }

    [HttpPost]
    [Route("v1/media/{mediaId}/views/increment")]
    public async Task<IActionResult> IncrementViews(
        [FromRoute] Guid mediaId,
        [FromServices] IMediator mediator)
    {
        await mediator.Send(new IncrementMediaViewsCommand(mediaId));

        return NoContent();
    }
}