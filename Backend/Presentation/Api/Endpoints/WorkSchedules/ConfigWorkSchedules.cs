using Api.ApiConfiguration;
using Api.ApiResults;
using Api.Setups;
using Application.UseCases.WorkSchedules;
using Application.UseCases.WorkSchedules.ConfigWorkSchedule;
using Domain.Abstractions.Errors;
using MediatR;
using Security.Setups;

namespace Api.Endpoints.WorkSchedules;

internal sealed record ConfigWorkSchedulesRequest(
    TimeOnly StartTime,
    TimeOnly EndTime, 
    int AppointmentDuration, 
    int[] Workdays
);

internal sealed class ConfigWorkSchedules : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPatch("work-schedules/{doctorId:guid}", async (
            Guid doctorId, 
            ConfigWorkSchedulesRequest request,
            ISender sender) =>
        {
            var command = new ConfigConfigWorkScheduleCommand(
                DoctorId: doctorId,
                StartTime: request.StartTime,
                EndTime: request.EndTime,
                AppointmentDuration: request.AppointmentDuration,
                Workdays: request.Workdays
            );
            Result<WorkScheduleResponse> result = await sender.Send(command);
            
            return result.Match(Results.Ok, ApiResults.ApiResults.Problem);
        })
        .RequireAuthorization(AuthTags.DoctorOrManagerPolicy)
        .RequireRateLimiting(RateLimiterSetup.FixedRateLimiter)
        .WithTags(EndpointTags.WorkSchedules)
        .Produces<WorkScheduleResponse>()
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound);
    }
}