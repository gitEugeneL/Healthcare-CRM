using Api.ApiConfiguration;
using Api.ApiResults;
using Application.UseCases.Doctors;
using Application.UseCases.Doctors.ChangeDoctorStatus;
using Domain.Abstractions.Errors;
using MediatR;
using Security.Setups;

namespace Api.Endpoints.Doctors;

internal sealed class ChangeDoctorStatus : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPatch("doctor/{doctorId:guid}/status", async (Guid doctorId, ISender sender) =>
            {
                var command = new ChangeDoctorStatusCommand(doctorId);
                Result<DoctorResponse> result = await sender.Send(command);
            
                return result.Match(Results.Ok, ApiResults.ApiResults.Problem);
            })
            .RequireAuthorization(AuthTags.DoctorOrManagerPolicy)
            .WithTags(EndpointTags.Doctors)
            .Produces<DoctorResponse>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);
    }
}