using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MovieHub.Engine.Domain.Exceptions;

namespace MovieHub.Engine.Api.Middleware;

public static class ProblemDetailsFactoryExtension
{
    public static ProblemDetails CreateFrom(this ProblemDetailsFactory problemDetailsFactory, HttpContext context,
        ValidationException validationException)
    {
        ModelStateDictionary modelStateDictionary = new ModelStateDictionary();

        foreach (var error in validationException.Errors)
        {
            modelStateDictionary.AddModelError(error.PropertyName, error.ErrorMessage);
        }

        ProblemDetails problemDetails = problemDetailsFactory.CreateValidationProblemDetails(
            context,
            modelStateDictionary,
            StatusCodes.Status400BadRequest,
            "Bad request");

        return problemDetails;
    }

    public static ProblemDetails CreateFrom(this ProblemDetailsFactory problemDetailsFactory, HttpContext context,
        DomainException domainException)
    {
        return problemDetailsFactory.CreateProblemDetails(
            context,
            domainException.ErrorCode switch
            {
                ErrorCode.Gone => StatusCodes.Status410Gone,
                ErrorCode.Conflict => StatusCodes.Status409Conflict,
                _ => throw new ArgumentOutOfRangeException()
            },
            "Error",
            detail: domainException.Message);
    }
}