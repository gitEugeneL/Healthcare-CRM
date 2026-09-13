using Api.ApiConfiguration;
using Api.ApiResults;
using Api.Setups;
using Application.UseCases.Patients.DeactivatePatient;
using Domain.Abstractions.Errors;
using MediatR;
using Security.Setups;

namespace Api.Endpoints.Patients;

internal sealed class DeactivatePatient : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("patients/{patientId:guid}", async (Guid patientId, ISender sender) =>
            {
                var command = new DeactivatePatientCommand(patientId);
                
                Result<Unit> result = await sender.Send(command);
                return result.Match(Results.Ok, ApiResults.ApiResults.Problem);
                
            })
            .RequireAuthorization(AuthTags.DoctorOrManagerPolicy)
            .RequireRateLimiting(RateLimiterSetup.FixedRateLimiter)
            .WithTags(EndpointTags.Patients)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status409Conflict);
    }
}