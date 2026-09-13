using Api.ApiConfiguration;
using Api.ApiResults;
using Application.UseCases.Security.ChangeEmail;
using Domain.Abstractions.Errors;
using MediatR;
using Security.Setups;
using Security.Utils;

namespace Api.Endpoints.Security;

internal sealed record ChangeEmailRequest(
    string NewEmail,
    string Code);

internal sealed class ChangeEmail : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("auth/change-email", async (ChangeEmailRequest request, HttpContext httpContext, ISender sender) =>
            {
                var command = new ChangeEmailCommand(
                    CurrentUserEmail: TokenReader.ReadUserEmailFromToken(httpContext),
                    NewEmail: request.NewEmail,
                    ConfirmationCode: request.Code);

                Result<Unit> result = await sender.Send(command);
                return result.Match(Results.Ok, ApiResults.ApiResults.Problem);
            })
            .RequireAuthorization(AuthTags.BasePolicy)
            .WithTags(EndpointTags.Security)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status409Conflict);
    }
}