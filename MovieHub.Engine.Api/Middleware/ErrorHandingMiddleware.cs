using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using MovieHub.Engine.Domain.Exceptions;

namespace MovieHub.Engine.Api.Middleware;

public class ErrorHandingMiddleware : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        ProblemDetailsFactory problemDetailsFactory = httpContext.RequestServices.GetRequiredService<ProblemDetailsFactory>();
        ILogger<ErrorHandingMiddleware> logger = httpContext.RequestServices.GetRequiredService<ILogger<ErrorHandingMiddleware>>();

        ProblemDetails problemDetails;
        switch (exception)
        {
            case ValidationException validationException:
                problemDetails = problemDetailsFactory.CreateFrom(httpContext, validationException);
                break;
            case DomainException domainException:
                problemDetails = problemDetailsFactory.CreateFrom(httpContext, domainException);

                logger.LogError(domainException, "domain exception");
                break;
            default:
                problemDetails = problemDetailsFactory.CreateProblemDetails(
                    httpContext,
                    StatusCodes.Status500InternalServerError,
                    "Unhandled error",
                    detail: exception.Message);

                logger.LogError(exception, "Unhandled exception");
                break;
        }

        httpContext.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, problemDetails.GetType(), cancellationToken: cancellationToken);

        return true;
    }
}