using Microsoft.AspNetCore.Mvc;
using MovieHub.Engine.Storage.Common;

namespace MovieHub.Engine.Api.Controllers;

[ApiController]
[Route("api/v1/application")]
public class ApplicationController: ControllerBase
{
    [HttpPut]
    [Route("seed")]
    public async Task<IActionResult> SeedGenerate(
        [FromServices] ISeedGenerator seedGenerator,
        CancellationToken cancellationToken)
    {
        await seedGenerator.GenerateSeed(cancellationToken);
        return Ok("Seed generated");
    }
}