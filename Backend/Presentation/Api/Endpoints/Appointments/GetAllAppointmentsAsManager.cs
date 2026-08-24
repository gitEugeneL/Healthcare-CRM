using Api.ApiConfiguration;
using Api.ApiResults;
using Api.Utils;
using Application.UseCases.Appointments;
using Application.UseCases.Appointments.GetAllByDate;
using MediatR;

namespace Api.Endpoints.Appointments;

internal sealed record GetAllAppointmentsAsManagerQueryParams(
    DateOnly Date,
    Guid? DoctorId,
    Guid? PatientId);

internal sealed class GetAllAppointmentsAsManager : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("appointments/manager", async (
                [AsParameters] GetAllAppointmentsAsManagerQueryParams queryParams,
                ISender sender) =>
            {
                var query = new GetAllAppointmentsByDateQuery(
                    Date: queryParams.Date,
                    DoctorId: queryParams.DoctorId,
                    PatientId: queryParams.PatientId);

                var result = await sender.Send(query);
                return result.Match(Results.Ok, ApiResults.ApiResults.Problem);
            })
            // TODO .RequireAuthorization(AuthTags.ManagerPolicy)
            .WithTags(ApiTags.Appointments)
            .Produces<IReadOnlyList<AppointmentResponse>>();
    }
}