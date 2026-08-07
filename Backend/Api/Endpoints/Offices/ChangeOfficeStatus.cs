using Api.ApiConfiguration;
using Api.ApiResults;
using Application.UseCases.Offices;
using Application.UseCases.Offices.ChangeOfficeStatus;
using Domain.Abstractions.Errors;
using MediatR;

namespace Api.Endpoints.Offices;

internal class ChangeOfficeStatus : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPatch("offices/{officeId:guid}/status", async (Guid officeId, ISender sender) =>
        {
            var command = new ChangeOfficeStatusCommand(officeId);
            Result<OfficeResponse> result = await sender.Send(command);
            
            return result.Match(Results.Ok, ApiResults.ApiResults.Problem);
            
        })
            // TODO .RequireAuthorization(AuthTags.DoctorOrManagerPolicy)
            .WithTags(ApiTags.Offices)
            .Produces<OfficeResponse>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);
    }
}