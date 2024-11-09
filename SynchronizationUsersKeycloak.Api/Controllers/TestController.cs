
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SynchronizationUsersKeycloak.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    [HttpGet]
    [Route("test1")]
    public async Task<IActionResult> Get1(
        CancellationToken cancellationToken)
    {
        return Ok(await Task.FromResult("success"));
    }
    
    [HttpGet]
    [Route("test2")]
    [Authorize]
    public async Task<IActionResult> Get2(
        CancellationToken cancellationToken)
    {
        return Ok(await Task.FromResult("success"));
    }

    [HttpGet]
    [Route("test3")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Get3(
        CancellationToken cancellationToken)
    {
        return Ok(await Task.FromResult("success"));
    }
}