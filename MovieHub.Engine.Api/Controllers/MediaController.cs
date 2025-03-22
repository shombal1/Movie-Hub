using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MovieHub.Engine.Api.Models.Requests;
using MovieHub.Engine.Api.Models.Responses;
using MovieHub.Engine.Domain.UseCases.GetMedia;
using MovieHub.Engine.Domain.UseCases.IncrementMediaViews;
using ParameterSorting = MovieHub.Engine.Api.Enums.ParameterSorting;
using TypeSorting = MovieHub.Engine.Api.Enums.TypeSorting;

namespace MovieHub.Engine.Api.Controllers;

[ApiController]
[Route("api/v1/media")]
public class MediaController : ControllerBase
{
    [HttpGet]
    [Route("{page}")]
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

    [HttpPost]
    [Route("{mediaId}/views/increment")]
    public async Task<IActionResult> IncrementViews(Guid mediaId,[FromServices] IMediator mediator)
    {
        await mediator.Send(new IncrementMediaViewsCommand(mediaId));

        return NoContent();
    }
    
}