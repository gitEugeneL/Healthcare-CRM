using Api.ApiConfiguration;
using Api.ApiResults;
using Application.UseCases.Security;
using Application.UseCases.Security.Login;
using Domain.Abstractions.Errors;
using MediatR;
using Security.Utils;

namespace Api.Endpoints.Security;

internal sealed record LoginRequest(string Email, string Password);

internal sealed record LoginResponse(string AccessToken, DateTime AccessTokenExpires);

internal sealed class Login : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("auth/login", async (LoginRequest request, HttpContext httpContext, ISender sender) =>
        {
            var command = new LoginCommand(
                Email: request.Email,
                Password: request.Password);
            
            Result<LoginOrRefreshResponse> result = await sender.Send(command);
            return result.Match(
                r =>
                {
                    CookieManager.SetCookie(
                        cookieName: CookieManager.RefreshToken,
                        context: httpContext,
                        value: r.RefreshToken.Token,
                        expires: r.RefreshToken.Expires);
                        
                    return Results.Ok(new LoginResponse(r.AccessToken, r.AccessTokenExpires));
                }, 
                ApiResults.ApiResults.Problem);
        })
        .AllowAnonymous()
        .WithTags(EndpointTags.Security)
        .Produces<LoginOrRefreshResponse>()
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound);
    }
}