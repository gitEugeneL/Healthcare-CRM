using Domain.Abstractions.Errors;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Behaviors;

internal sealed class RequestLoggingPipelineBehavior<TRequest, TResponse>(
    ILogger<RequestLoggingPipelineBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class
    where TResponse : Result
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;

        logger.LogInformation("Processing request {RequestName}", requestName);

        var result = await next(cancellationToken);

        if (result.IsSuccess)
        {
            logger.LogInformation("Completed request {RequestName}", requestName);
        }
        else
        {
            logger.LogError("Completed request {RequestName} with error: {@Error}", requestName, result.Error);
        }

        return result;
    }
}