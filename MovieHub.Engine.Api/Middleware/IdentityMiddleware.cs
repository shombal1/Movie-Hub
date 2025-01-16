using System.Security.Claims;
using MovieHub.Engine.Domain.Authentication;

namespace MovieHub.Engine.Api.Middleware;

public class IdentityMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext httpContext, IIdentityProvider identityProvider)
    {
        var user = httpContext.User;

        if (user.Identity != null && user.Identity.IsAuthenticated)
        {
            identityProvider.Current =
                new User(Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value!), user.Identity.IsAuthenticated);
        }
        else
        {
            identityProvider.Current = new User(Guid.Empty, false);
        }

        await next.Invoke(httpContext);
    }
}