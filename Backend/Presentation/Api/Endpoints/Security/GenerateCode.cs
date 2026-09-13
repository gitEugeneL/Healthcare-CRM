using Api.ApiConfiguration;
using Api.ApiResults;
using Api.Setups;
using Application.UseCases.Security.GenerateCode;
using Domain.Abstractions.Errors;
using MediatR;

namespace Api.Endpoints.Security;

internal sealed record GenerateCodeRequest(string Email);

internal sealed class GenerateCode : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("auth/generate-code", async (GenerateCodeRequest request, ISender sender) =>
        {
            var command = new GenerateCodeCommand(request.Email);
            
            Result<GenerateCodeResponse> result = await sender.Send(command);
            return result.Match(Results.Ok, ApiResults.ApiResults.Problem);
        })   
        .AllowAnonymous()
        .RequireRateLimiting(RateLimiterSetup.FixedRateLimiter)
        .WithTags(EndpointTags.Security)
        .Produces<GenerateCodeResponse>()
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound);
    }
}