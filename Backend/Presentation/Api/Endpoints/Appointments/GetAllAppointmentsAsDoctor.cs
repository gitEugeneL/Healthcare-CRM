using Api.ApiConfiguration;
using Api.ApiResults;
using Application.UseCases.Appointments;
using Application.UseCases.Appointments.GetAllAppointments;
using Application.UseCases.Common.Pagination;
using MediatR;
using Security.Setups;
using Security.Utils;

namespace Api.Endpoints.Appointments;

internal sealed record GetAllAppointmentsAsDoctorQueryParams(
    int? PageNumber,
    int? PageSize,
    DateOnly? Date,
    Guid? PatientId);

internal sealed class GetAllAppointmentsAsDoctor : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("appointments/doctor", async (
                [AsParameters] GetAllAppointmentsAsDoctorQueryParams queryParams,
                HttpContext httpContext,
                ISender sender) =>
            {
               var currentDoctorId = TokenReader.ReadUserIdFromToken(httpContext);
                
                var query = new GetAllAppointmentsQuery(
                    PageNumber: queryParams.PageNumber,
                    PageSize: queryParams.PageSize,
                    Date: queryParams.Date,
                    DoctorId: currentDoctorId,
                    PatientId: queryParams.PatientId);

                var result = await sender.Send(query);
                return result.Match(Results.Ok, ApiResults.ApiResults.Problem);
            })
            .RequireAuthorization(AuthTags.DoctorPolicy)
            .WithTags(EndpointTags.Appointments)
            .Produces<PaginationResult<AppointmentResponse>>();
    }
}