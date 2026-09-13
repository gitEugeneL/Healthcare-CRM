using Api.ApiConfiguration;
using Api.ApiResults;
using Api.Setups;
using Application.UseCases.Appointments;
using Application.UseCases.Appointments.GetAppointmentSlots;
using MediatR;
using Security.Setups;

namespace Api.Endpoints.Appointments;

internal sealed record GetAppointmentSlotsQueryParams(DateOnly? Date = null); 

internal sealed class GetAppointmentSlots : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("appointment/date/doctor-slots/{doctorId:guid}", async (
                Guid doctorId,
                [AsParameters] GetAppointmentSlotsQueryParams queryParams, 
                ISender sender) =>
            {
                var query = new GetAppointmentSlotsQuery(
                    DoctorId: doctorId,
                    Date: queryParams.Date ?? DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)));
                
                var result = await sender.Send(query);
                return result.Match(Results.Ok, ApiResults.ApiResults.Problem);
            })
            .RequireAuthorization(AuthTags.ManagerOrPatientPolicy)
            .RequireRateLimiting(RateLimiterSetup.FixedRateLimiter)
            .WithTags(EndpointTags.Appointments)
            .Produces<AppointmentSlotsResponse>()
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest);
    }
}