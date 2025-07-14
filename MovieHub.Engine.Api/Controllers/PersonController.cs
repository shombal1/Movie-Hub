using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MovieHub.Engine.Api.Models.Responses;
using MovieHub.Engine.Domain.UseCases.GetBaseInfoPersons;
using MovieHub.Engine.Domain.UseCases.GetPerson;

namespace MovieHub.Engine.Api.Controllers;

[ApiController]
[Route("api/person")]
public class PersonController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [Route("{personId}")]
    public async Task<IActionResult> GetPerson(
        [FromRoute] Guid personId,
        [FromServices] IMapper mapper,
        CancellationToken cancellationToken)
    {
        var query = new GetPersonQuery(personId);
        var result = await mediator.Send(query, cancellationToken);

        return Ok(mapper.Map<PersonDto>(result));
    }
    
    [HttpGet]
    [Route("base-info/{page}")]
    public async Task<IActionResult> GetBaseInfoPersons(
        [FromRoute] int page,
        [FromServices] IMapper mapper,
        CancellationToken cancellationToken)
    {
        var query = new GetBaseInfoPersonsQuery(page);
        var result = await mediator.Send(query, cancellationToken);
        
        return Ok(mapper.Map<IEnumerable<BasePersonInfoDto>>(result));
    }
}