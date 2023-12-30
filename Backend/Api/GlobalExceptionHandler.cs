using Application.Common.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace Api;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var (statusCodes, errorMessage) = exception switch
        {
            UnauthorizedException => (401, exception.Message),
            AccessDeniedException => (403, exception.Message),
            NotFoundException => (404, exception.Message),
            AlreadyExistException => (409, exception.Message),
            TimeMismatchException => (422, exception.Message),
            _ => (500, "Something went wrong")
        };

        httpContext.Response.StatusCode = statusCodes;
        await httpContext.Response.WriteAsJsonAsync(errorMessage, cancellationToken);
        return true;
    }
}
