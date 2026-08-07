using Api.ApiConfiguration;
using Api.ApiResults;
using Application.UseCases.Managers;
using Application.UseCases.Managers.UpdateManager;
using Domain.Abstractions.Errors;
using MediatR;

namespace Api.Endpoints.Managers;

internal sealed record UpdateManagerRequest(
    Guid UserId,
    string? Phone,
    string? Position,
    string? FirstName,
    string? LastName
);

public class UpdateManager : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPatch("manager", async (UpdateManagerRequest request, ISender sender) =>
            {
                var command = new UpdateManagerCommand(
                    UserId: request.UserId,
                    Phone: request.Phone,
                    Position: request.Position,
                    FirstName: request.FirstName,
                    LastName: request.LastName
                );
                Result<ManagerResponse> result = await sender.Send(command);
                
                return result.Match(Results.Ok, ApiResults.ApiResults.Problem);
            }) 
            // TODO .RequireAuthorization(AuthTags.AdminPolicy)
            .WithTags(ApiTags.Managers)
            .Produces<ManagerResponse>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);
            
    }
}