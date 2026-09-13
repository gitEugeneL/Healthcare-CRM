using Api.ApiConfiguration;
using Api.ApiResults;
using Api.Setups;
using Application.UseCases.Security.Logout;
using Domain.Abstractions.Errors;
using MediatR;
using Security.Utils;

namespace Api.Endpoints.Security;

internal sealed record LogoutRequest(string Email);

internal sealed class Logout : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("auth/logout", async (LogoutRequest request, HttpContext httpContext, ISender sender) =>
            {
                var command = new LogoutCommand(
                    Email: request.Email,
                    RefreshTokenValue: CookieManager.ReadCookie(CookieManager.RefreshToken, httpContext)!);
                
                Result<Unit> result = await sender.Send(command);
                return result.Match(
                    r =>
                    {
                        CookieManager.RemoveCookie(CookieManager.RefreshToken, httpContext);
                        return Results.NoContent();
                    }, 
                    ApiResults.ApiResults.Problem);
            })
            .AllowAnonymous()
            .RequireRateLimiting(RateLimiterSetup.FixedRateLimiter)
            .WithTags(EndpointTags.Security)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);
    }
}