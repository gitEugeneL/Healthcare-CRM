using Api.ApiConfiguration;
using Api.ApiResults;
using Application.UseCases.Appointments;
using Application.UseCases.Appointments.CreateAppointment;
using MediatR;
using Security.Setups;
using Security.Utils;

namespace Api.Endpoints.Appointments;

internal sealed record CreateAppointmentAsDoctorRequest(
    Guid PatientId,
    DateOnly Date,
    TimeOnly StartTime
);

internal sealed class CreateAppointmentAsDoctor : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("appointments/doctor", async (
                CreateAppointmentAsDoctorRequest request,
                HttpContext httpContext,
                ISender sender) =>
        {
            var currentDoctorId = TokenReader.ReadUserIdFromToken(httpContext);
            
            var command = new CreateAppointmentCommand(
                PatientId: request.PatientId,
                DoctorId: currentDoctorId,
                Date: request.Date,
                StartTime: request.StartTime);
            
            var result = await sender.Send(command);
            return result.Match(Results.Ok, ApiResults.ApiResults.Problem);

        }) 
        .RequireAuthorization(AuthTags.DoctorPolicy)
        .WithTags(EndpointTags.Appointments)
        .Produces<AppointmentResponse>()
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound);
    }
}