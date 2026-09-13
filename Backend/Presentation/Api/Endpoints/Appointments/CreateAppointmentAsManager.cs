using Api.ApiConfiguration;
using Api.ApiResults;
using Api.Setups;
using Application.UseCases.Appointments;
using Application.UseCases.Appointments.CreateAppointment;
using MediatR;
using Security.Setups;

namespace Api.Endpoints.Appointments;

internal sealed record CreateAppointmentAsManagerRequest(
    Guid PatientId,
    Guid DoctorId,
    DateOnly Date,
    TimeOnly StartTime
);

internal sealed class CreateAppointmentAsManager : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("appointments/manager", async (
                CreateAppointmentAsManagerRequest request,
                ISender sender) =>
            {
                var command = new CreateAppointmentCommand(
                    PatientId: request.PatientId,
                    DoctorId: request.DoctorId,
                    Date: request.Date,
                    StartTime: request.StartTime);
                
                var result = await sender.Send(command);
                return result.Match(Results.Ok, ApiResults.ApiResults.Problem);
            })
            .RequireAuthorization(AuthTags.ManagerPolicy)
            .RequireRateLimiting(RateLimiterSetup.FixedRateLimiter)
            .WithTags(EndpointTags.Appointments)
            .Produces<AppointmentResponse>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);
    }
}