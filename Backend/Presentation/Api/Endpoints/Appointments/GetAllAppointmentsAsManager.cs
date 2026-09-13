using Api.ApiConfiguration;
using Api.ApiResults;
using Application.UseCases.Appointments;
using Application.UseCases.Appointments.GetAllAppointments;
using Application.UseCases.Common.Pagination;
using MediatR;
using Security.Setups;

namespace Api.Endpoints.Appointments;

internal sealed record GetAllAppointmentsAsManagerQueryParams(
    int? PageNumber,
    int? PageSize,
    DateOnly? Date,
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
                var query = new GetAllAppointmentsQuery(
                    PageNumber: queryParams.PageNumber,
                    PageSize: queryParams.PageSize,
                    Date: queryParams.Date,
                    DoctorId: queryParams.DoctorId,
                    PatientId: queryParams.PatientId);

                var result = await sender.Send(query);
                return result.Match(Results.Ok, ApiResults.ApiResults.Problem);
            })
            .RequireAuthorization(AuthTags.ManagerPolicy)
            .WithTags(EndpointTags.Appointments)
            .Produces<PaginationResult<AppointmentResponse>>();
    }
}