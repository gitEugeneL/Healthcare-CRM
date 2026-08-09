using Domain.Abstractions.Errors;
using MediatR;

namespace Application.UseCases.WorkSchedules.ConfigWorkSchedule;

public sealed record ConfigConfigWorkScheduleCommand(
    Guid DoctorId,
    TimeOnly StartTime,
    TimeOnly EndTime,
    int AppointmentDuration,
    int[] Workdays
) : IRequest<Result<WorkScheduleResponse>>;

