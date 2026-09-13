using Api.ApiConfiguration;
using Api.ApiResults;
using Api.Setups;
using Application.UseCases.Appointments;
using Application.UseCases.Appointments.GetAllAppointments;
using Application.UseCases.Common.Pagination;
using MediatR;
using Security.Setups;
using Security.Utils;

namespace Api.Endpoints.Appointments;

internal sealed record GetAllAppointmentsAsPatientQueryParams(
    int? PageNumber,
    int? PageSize,
    DateOnly? Date,
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
            
                var query = new GetAllAppointmentsQuery(
                    PageNumber: queryParams.PageNumber,
                    PageSize: queryParams.PageSize,
                    Date: queryParams.Date,
                    DoctorId: queryParams.DoctorId,
                    PatientId: currentPatientId);

                var result = await sender.Send(query);
                return result.Match(Results.Ok, ApiResults.ApiResults.Problem);
            })
            .RequireAuthorization(AuthTags.PatientPolicy)
            .RequireRateLimiting(RateLimiterSetup.FixedRateLimiter)
            .WithTags(EndpointTags.Appointments)
            .Produces<PaginationResult<AppointmentResponse>>();
    }
}