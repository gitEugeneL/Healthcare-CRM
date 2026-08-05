using Api.ApiResults;
using Application.UseCases.Offices;
using Application.UseCases.Offices.UpdateOfficeName;
using Domain.Abstractions.Errors;
using MediatR;

namespace Api.Endpoints.Offices;

internal sealed record UpdateOfficeNameRequest(string Name);

internal static class UpdateOfficeName
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPatch("offices/{officeId:guid}/name", async (
                Guid officeId,
                UpdateOfficeNameRequest request, 
                ISender sender) =>
            {
                var command = new UpdateOfficeNameCommand(officeId, request.Name);
                Result<OfficeResponse> result = await sender.Send(command);
                
                return result.Match(Results.Ok, ApiResults.ApiResults.Problem);
            })
            // TODO .RequireAuthorization(AuthTags.ManagerPolicy)
            .WithTags(ApiTags.Offices)
            .Produces<OfficeResponse>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);
    }
}