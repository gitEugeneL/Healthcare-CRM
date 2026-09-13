using Api.ApiConfiguration;
using Api.ApiResults;
using Api.Setups;
using Application.UseCases.Offices;
using Application.UseCases.Offices.UpdateOfficeName;
using Domain.Abstractions.Errors;
using MediatR;
using Security.Setups;

namespace Api.Endpoints.Offices;

internal sealed record UpdateOfficeNameRequest(string Name);

internal class UpdateOfficeName : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
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
            .RequireAuthorization(AuthTags.ManagerPolicy)
            .RequireRateLimiting(RateLimiterSetup.FixedRateLimiter)
            .WithTags(EndpointTags.Offices)
            .Produces<OfficeResponse>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);
    }
}