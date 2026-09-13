using Api.ApiConfiguration;
using Api.ApiResults;
using Application.UseCases.Security;
using Application.UseCases.Security.Refresh;
using Domain.Abstractions.Errors;
using MediatR;
using Security.Utils;

namespace Api.Endpoints.Security;

internal sealed record RefreshRequest(string Email);

internal sealed class Refresh : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("auth/refresh", async (RefreshRequest request, HttpContext httpContext, ISender sender) =>
            {
                var command = new RefreshCommand(
                    RefreshTokenValue: CookieManager.ReadCookie(CookieManager.RefreshToken, httpContext)!,
                    Email: request.Email);

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