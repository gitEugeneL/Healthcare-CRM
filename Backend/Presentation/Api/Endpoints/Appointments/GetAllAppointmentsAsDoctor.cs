using Api.ApiConfiguration;
using Api.ApiResults;
using Api.Utils;
using Application.UseCases.Appointments;
using Application.UseCases.Appointments.GetAllByDate;
using MediatR;

namespace Api.Endpoints.Appointments;

internal sealed record GetAllAppointmentsAsDoctorQueryParams(
    DateOnly Date,
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
                
                var query = new GetAllAppointmentsByDateQuery(
                    Date: queryParams.Date,
                    DoctorId: currentDoctorId,
                    PatientId: queryParams.PatientId);

                var result = await sender.Send(query);
                return result.Match(Results.Ok, ApiResults.ApiResults.Problem);
            })
             // TODO .RequireAuthorization(AuthTags.DoctorPolicy)
            .WithTags(ApiTags.Appointments)
            .Produces<IReadOnlyList<AppointmentResponse>>();
    }
}