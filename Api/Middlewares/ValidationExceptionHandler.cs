using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace api.Middlewares;

public class ValidationExceptionHandler : IExceptionHandler
{
    private readonly IProblemDetailsService _problemDetailsService;

    public ValidationExceptionHandler(IProblemDetailsService problemDetailsService)
    {
        _problemDetailsService = problemDetailsService;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not ValidationException validationException) return false;

        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        ProblemDetailsContext context = new()
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails()
            {
                Detail = "One or more validation errors have occured",
                Status = httpContext.Response.StatusCode
            }
        };

        var errors = validationException.Errors;
        context.ProblemDetails.Extensions.Add("errors", errors);
        bool detailsResult = await _problemDetailsService.TryWriteAsync(context);

        return detailsResult;
    }
}