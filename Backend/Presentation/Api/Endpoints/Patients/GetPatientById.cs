using Api.ApiConfiguration;
using Api.ApiResults;
using Application.UseCases.Patients;
using Application.UseCases.Patients.GetPatientById;
using Domain.Abstractions.Errors;
using MediatR;

namespace Api.Endpoints.Patients;

internal sealed class GetPatientById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("patients/{patientId:guid}", async (Guid patientId, ISender sender) =>
            {
                var command = new GetPatientByIdQuery(patientId);
                
                Result<PatientResponse> result = await sender.Send(command);
                return result.Match(Results.Ok, ApiResults.ApiResults.Problem);
            })
            // TODO .RequireAuthorization(AuthTags.DoctorOrManagerPolicy)
            .WithTags(ApiTags.Patients)
            .Produces<PatientResponse>()
            .Produces(StatusCodes.Status404NotFound);
    }
}