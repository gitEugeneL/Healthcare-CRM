using Api.ApiConfiguration;
using Api.ApiResults;
using Application.UseCases.Managers;
using Application.UseCases.Managers.UpdateManager;
using Domain.Abstractions.Errors;
using MediatR;
using Security.Setups;

namespace Api.Endpoints.Managers;

internal sealed record UpdateManagerRequest(
    string? Phone,
    string? Position,
    string? FirstName,
    string? LastName
);

internal class UpdateManager : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPatch("managers/{managerId:guid}", async (
                Guid managerId, 
                UpdateManagerRequest request, 
                ISender sender) =>
            {
                var command = new UpdateManagerCommand(
                    ManagerId: managerId,
                    Phone: request.Phone,
                    Position: request.Position,
                    FirstName: request.FirstName,
                    LastName: request.LastName
                );
                Result<ManagerResponse> result = await sender.Send(command);
                
                return result.Match(Results.Ok, ApiResults.ApiResults.Problem);
            }) 
            .RequireAuthorization(AuthTags.AdminPolicy)
            .WithTags(EndpointTags.Managers)
            .Produces<ManagerResponse>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);
            
    }
}