using Api.ApiConfiguration;
using Api.ApiResults;
using Api.Utils;
using Application.UseCases.Appointments;
using Application.UseCases.Appointments.CreateAppointment;
using MediatR;

namespace Api.Endpoints.Appointments;

internal sealed record CreateAppointmentAsPatientRequest(
    Guid DoctorId,
    DateOnly Date,
    TimeOnly StartTime
);

internal sealed class CreateAppointmentAsPatient : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("appointments/patient", async (
                CreateAppointmentAsPatientRequest request,
                HttpContext httpContext,
                ISender sender) =>
        {
            var currentPatientId = TokenReader.ReadUserIdFromToken(httpContext);
            
            var command = new CreateAppointmentCommand(
                PatientId: currentPatientId,
                DoctorId: request.DoctorId,
                Date: request.Date,
                StartTime: request.StartTime);
            
            var result = await sender.Send(command);
            return result.Match(Results.Ok, ApiResults.ApiResults.Problem);
            
        })
        .RequireAuthorization(AuthTags.PatientPolicy)
        .WithTags(ApiTags.Appointments)
        .Produces<AppointmentResponse>()
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound);
    }
}