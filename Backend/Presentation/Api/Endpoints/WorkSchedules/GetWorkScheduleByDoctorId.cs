using Api.ApiConfiguration;
using Api.ApiResults;
using Application.UseCases.WorkSchedules;
using Application.UseCases.WorkSchedules.GetWorkScheduleByDoctorId;
using Domain.Abstractions.Errors;
using MediatR;

namespace Api.Endpoints.WorkSchedules;

internal sealed class GetWorkScheduleByDoctorId : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("work-schedules/{doctorId:guid}", async (Guid doctorId, ISender sender) =>
            {
                var query = new GetWorkScheduleByDoctorIdQuery(doctorId);
                Result<WorkScheduleResponse> result = await sender.Send(query);
                
                return result.Match(Results.Ok, ApiResults.ApiResults.Problem);
            })
        .AllowAnonymous()
        .WithTags(ApiTags.WorkSchedules)
        .Produces<WorkScheduleResponse>()
        .Produces(StatusCodes.Status404NotFound);
    }
}