using Api.ApiConfiguration;
using Api.ApiResults;
using Application.UseCases.Managers;
using Application.UseCases.Managers.CreateManager;
using Domain.Abstractions.Errors;
using MediatR;
using Security.Setups;

namespace Api.Endpoints.Managers;

internal sealed record CreateManagerRequest(
    string Email,
    string Password,
    string? Phone,
    string? FirstName,
    string? LastName,
    string? Position);

internal class CreateManager : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("managers", async (CreateManagerRequest request, ISender sender) =>
            {
                var command = new CreateMangerCommand(
                    Email: request.Email,
                    Password: request.Password,
                    Position: request.Position,
                    Phone: request.Phone,
                    FirstName: request.FirstName,
                    LastName: request.LastName);

                Result<ManagerResponse> result = await sender.Send(command);

                return result.Match(Results.Ok, ApiResults.ApiResults.Problem);

            })
            .RequireAuthorization(AuthTags.AdminPolicy)
            .WithTags(EndpointTags.Managers)
            .Produces<ManagerResponse>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status409Conflict);
    }
}