using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MovieHub.Domain.Authentication;
using MovieHub.Storage;
using User = MovieHub.Storage.Entities.User;

namespace MovieHub.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    [HttpGet]
    [Route("test1")]
    public async Task<IActionResult> Get1(
        MovieHubDbContext dbContext,
        CancellationToken cancellationToken)
    {
        await dbContext.Users.InsertOneAsync(new User()
        {
            Id = Guid.NewGuid()
        }, new InsertOneOptions(), cancellationToken);
        return Ok(dbContext.Users.AsQueryable().ToArray());
    }
    //     return Ok(await Task.FromResult(
    //         new Dictionary<string, string>(User.Claims.GroupBy(c => c.Type, c => c.Value)
    //             .Select(c => new KeyValuePair<string, string>(c.Key, string.Join(",", c.ToArray()))))));
    // }

    [HttpGet]
    [Route("test2")]
    [Authorize]
    public async Task<IActionResult> Get2(
        CancellationToken cancellationToken)
    {
        return Ok(await Task.FromResult(
            new Dictionary<string, string>(User.Claims.GroupBy(c => c.Type, c => c.Value)
                .Select(c => new KeyValuePair<string, string>(c.Key, string.Join(",", c.ToArray()))))));
    }

    [HttpGet]
    [Route("test3")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Get3(
        IIdentityProvider identityProvider,
        CancellationToken cancellationToken)
    {
        
        return Ok(await Task.FromResult(identityProvider.Current.Id));
        //     return Ok(await Task.FromResult(
        //         new Dictionary<string, string>(User.Claims.GroupBy(c => c.Type, c => c.Value)
        //             .Select(c => new KeyValuePair<string, string>(c.Key, string.Join(",", c.ToArray()))))));
    }
}