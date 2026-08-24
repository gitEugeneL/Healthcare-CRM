using Api.ApiConfiguration;
using Api.ApiResults;
using Api.Utils;
using Application.UseCases.Appointments;
using Application.UseCases.Appointments.GetAllByDate;
using MediatR;

namespace Api.Endpoints.Appointments;

internal sealed record GetAllAppointmentsAsPatientQueryParams(
    DateOnly Date,
    Guid? DoctorId);

internal sealed class GetAllAppointmentsAsPatient : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("appointments/patient", async (
                [AsParameters] GetAllAppointmentsAsPatientQueryParams queryParams,
                HttpContext httpContext,
                ISender sender) =>
            {
                var currentPatientId = TokenReader.ReadUserIdFromToken(httpContext);
            
                var query = new GetAllAppointmentsByDateQuery(
                    Date: queryParams.Date,
                    DoctorId: queryParams.DoctorId,
                    PatientId: currentPatientId);

                var result = await sender.Send(query);
                return result.Match(Results.Ok, ApiResults.ApiResults.Problem);
            })
            // TODO .RequireAuthorization(AuthTags.PatientPolicy)
            .WithTags(ApiTags.Appointments)
            .Produces<IReadOnlyList<AppointmentResponse>>();
    }
}