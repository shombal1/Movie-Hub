using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieHub.Engine.Api.Models.Requests;
using MovieHub.Engine.Api.Models.Responses;
using MovieHub.Engine.Domain.UseCases.AddMediaToBasket;
using MovieHub.Engine.Domain.UseCases.GetMediaFromBasket;
using MovieHub.Engine.Domain.UseCases.RemoveMediaFromBasket;

namespace MovieHub.Engine.Api.Controllers;

[ApiController]
[Route("api/v1/media-basket")]
public class MediaBasketController : ControllerBase
{
    [HttpGet]
    [Authorize]
    [Route("{page}")]
    public async Task<IActionResult> GetMediaFromBasket([FromServices] IMediator mediator,
        [FromServices] IMapper mapper, [FromRoute] int page,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetMediaFromBasketQuery(page), cancellationToken);
        
        return Ok(mapper.Map<IEnumerable<MediaDto>>(result));
    }
    
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> TryAddMediaToBasket(
        [FromServices] IMediator mediator,
        [FromBody] AddMediaToBasketDto request,
        CancellationToken cancellationToken)
    {
        await mediator.Send(new AddMediaToBasketCommand(request.MediaId), cancellationToken);
        return Ok(new { message = "Media successfully added to basket" });
    }

    [HttpDelete]
    [Authorize]
    [Route("{mediaId}")]
    public async Task<IActionResult> RemoveMediaFromBasket([FromServices] IMediator mediator,Guid mediaId)
    {
        await mediator.Send(new RemoveMediaFromBasketCommand(mediaId));
        return Ok(new { message = "Media successfully removed from basket" });
    }
}