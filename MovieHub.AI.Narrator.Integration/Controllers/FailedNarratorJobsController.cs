using Microsoft.AspNetCore.Mvc;
using MovieHub.AI.Narrator.Domain.Models;
using MovieHub.AI.Narrator.Domain.UseCases.GetFailedNarratorJobs;
using MovieHub.AI.Narrator.Domain.UseCases.RetryFailedNarratorJob;

namespace MovieHub.AI.Narrator.Integration.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FailedNarratorJobsController : ControllerBase
{

    [HttpGet]
    public async Task<ActionResult<IEnumerable<FailedNarratorJob>>> GetFailedJobs(
        [FromQuery] int page,
        [FromServices] IGetFailedNarratorJobsUseCase useCase,
        CancellationToken cancellationToken)
    {
        var query = new GetFailedNarratorJobsQuery(page);
        var result = await useCase.Get(query, cancellationToken);
        return Ok(result);
    }

    [HttpPost("{jobId}/retry")]
    public async Task<ActionResult> RetryFailedJob(
        Guid jobId,
        [FromServices] IRetryFailedNarratorJobUseCase useCase,
        CancellationToken cancellationToken)
    {
        var command = new RetryFailedNarratorJobCommand(jobId);
        await useCase.RetryFailedJob(command, cancellationToken);
        return Ok();
    }
}