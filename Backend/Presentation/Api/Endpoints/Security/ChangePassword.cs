using Api.ApiConfiguration;
using Api.ApiResults;
using Api.Setups;
using Application.UseCases.Security.ChangePassword;
using Domain.Abstractions.Errors;
using MediatR;

namespace Api.Endpoints.Security;

internal sealed record ChangePasswordRequest(
    string Email, 
    string Code, 
    string Password, 
    string PasswordConfirmation);

internal sealed class ChangePassword : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("auth/change-password", async (ChangePasswordRequest request, ISender sender) =>
            {
                var command = new ChangePasswordCommand(
                    Email: request.Email,
                    ConfirmationCode: request.Code,
                    NewPassword: request.Password,
                    NewPasswordConfirmation: request.PasswordConfirmation);

                Result<Unit> result = await sender.Send(command);
                return result.Match(Results.Ok, ApiResults.ApiResults.Problem);
            })
            .AllowAnonymous()
            .RequireRateLimiting(RateLimiterSetup.FixedRateLimiter)
            .WithTags(EndpointTags.Security)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);
    }
}