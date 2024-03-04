using Api.Utils;
using API.Utils;
using Application.Common.Exceptions;
using Application.Common.Models;
using Application.Operations.Auth.Commands.Login;
using Application.Operations.Auth.Commands.Logout;
using Application.Operations.Auth.Commands.Refresh;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints.v1;

public class AuthenticationEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1/auth")
            .WithTags("Authentication");
        
        group.MapPost("login", Login)
            .WithValidator<LoginCommand>()
            .Produces<JwtToken>()
            .Produces(StatusCodes.Status400BadRequest);

        group.MapPost("refresh", Refresh)
            .Produces<JwtToken>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized);

        group.MapPost("logout", Logout)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status401Unauthorized);
    }

    private async Task<Results<Ok<JwtToken>, BadRequest<string>>> Login(
        [FromBody] LoginCommand command,
        ISender sender,
        HttpContext httpContext)
    {
        try
        {
            var result = await sender.Send(command);
            CookieManager
                .SetCookie(
                    httpContext.Response, 
                    "refreshToken", 
                    result.CookieToken.Token,
                    result.CookieToken.Expires);
            return TypedResults.Ok(result.JwtToken);
        }
        catch (AccessDeniedException exception)
        {
            return TypedResults.BadRequest(exception.Message);
        }
    }

    private async Task<Results<Ok<JwtToken>, BadRequest, UnauthorizedHttpResult>> Refresh(
        ISender sender,
        HttpContext httpContext)
    {
        try
        {
            var userRefreshToken = httpContext.Request.Cookies["refreshToken"];
            if (userRefreshToken is null)
                return TypedResults.BadRequest();
            var result = await sender.Send(new RefreshCommand(userRefreshToken));
            CookieManager
                .SetCookie(
                    httpContext.Response, 
                    "refreshToken", 
                    result.CookieToken.Token, 
                    result.CookieToken.Expires);
            return TypedResults.Ok(result.JwtToken);
        }
        catch (UnauthorizedException exception)
        {
            return TypedResults.Unauthorized();
        }
    }

    private async Task<Results<NoContent, UnauthorizedHttpResult, BadRequest>> Logout(
        ISender sender,
        HttpContext httpContext)
    {
        try
        {
            var userRefreshToken = httpContext.Request.Cookies["refreshToken"];
            if (userRefreshToken is null)
                return TypedResults.BadRequest();
            await sender.Send(new LogoutCommand(userRefreshToken));
            CookieManager.RemoveCookie(httpContext.Response, "refreshToken");
            return TypedResults.NoContent();
        }
        catch (UnauthorizedException exception)
        {
            return TypedResults.Unauthorized();
        }
    }
}