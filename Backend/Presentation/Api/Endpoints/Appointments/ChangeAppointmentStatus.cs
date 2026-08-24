using Api.ApiConfiguration;
using Api.ApiResults;
using Application.UseCases.Appointments;
using Application.UseCases.Appointments.ChangeAppointmentStatus;
using MediatR;

namespace Api.Endpoints.Appointments;

internal sealed record ChangeAppointmentStatusRequest(string Status);

internal sealed class ChangeAppointmentStatus : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPatch("appointments/{appointmentId:guid}/status", async (
                Guid appointmentId,
                ChangeAppointmentStatusRequest request,
                ISender sender) =>
            {
                var command = new ChangeAppointmentStatusCommand(appointmentId, request.Status);
                
                var result = await sender.Send(command);
                return result.Match(Results.Ok, ApiResults.ApiResults.Problem);
            })
            // TODO .RequireAuthorization(AuthTags.DoctorOrManagerPolicy)
            .WithTags(ApiTags.Appointments)
            .Produces<AppointmentResponse>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);
    }
}