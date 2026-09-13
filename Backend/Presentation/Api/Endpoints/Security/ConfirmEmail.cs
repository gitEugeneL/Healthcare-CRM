using Api.ApiConfiguration;
using Api.ApiResults;
using Application.UseCases.Security.ConfirmEmail;
using Domain.Abstractions.Errors;
using MediatR;

namespace Api.Endpoints.Security;

internal sealed record ConfirmEmailRequest(string Email, string Code);

internal sealed class ConfirmEmail : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPatch("auth/confirm-email", async (ConfirmEmailRequest request, ISender sender) =>
            {
                var command = new ConfirmEmailCommand(
                    Email: request.Email,
                    ConfirmationCode: request.Code); 
                
                Result<Unit> result = await sender.Send(command); 
                return result.Match(Results.Ok, ApiResults.ApiResults.Problem);
        })
        .AllowAnonymous()
        .WithTags(EndpointTags.Security)
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound);
    }
}