using Domain.Abstractions.Errors;
using Domain.WorkSchedules;
using MediatR;

namespace Application.UseCases.WorkSchedules.GetWorkScheduleByDoctorId;

internal sealed class GetWorkScheduleByDoctorIdQueryHandler(
    IWorkScheduleRepository workScheduleRepository   
) : IRequestHandler<GetWorkScheduleByDoctorIdQuery, Result<WorkScheduleResponse>>
{
    public async Task<Result<WorkScheduleResponse>> Handle(GetWorkScheduleByDoctorIdQuery query, CancellationToken ct)
    {
        var workSchedule = await workScheduleRepository.FindWorkScheduleByDoctorId(query.DoctorId, ct);
        
        return workSchedule is null
            ? Result.Failure<WorkScheduleResponse>(WorkScheduleErrors.NotFound(query.DoctorId)) 
            : WorkScheduleResponse.FromWorkSchedule(workSchedule);
    }
}
