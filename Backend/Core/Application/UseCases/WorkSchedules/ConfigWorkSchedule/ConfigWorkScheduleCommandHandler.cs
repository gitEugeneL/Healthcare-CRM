using Domain.Abstractions;
using Domain.Abstractions.Errors;
using Domain.Doctors;
using Domain.WorkSchedules;
using MediatR;
namespace Application.UseCases.WorkSchedules.ConfigWorkSchedule;

internal sealed class ConfigConfigWorkScheduleCommandHandler(
    IDoctorRepository doctorRepository,
    IWorkScheduleRepository workScheduleRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<ConfigConfigWorkScheduleCommand, Result<WorkScheduleResponse>>
{
    public async Task<Result<WorkScheduleResponse>> Handle(ConfigConfigWorkScheduleCommand command, CancellationToken ct)
    {
        var doctorExists = await doctorRepository.DoctorExistsAsync(command.DoctorId, ct);
        if (!doctorExists)
            return Result.Failure<WorkScheduleResponse>(DoctorErrors.NotFound(command.DoctorId));

        var appointmentDuration = (AppointmentDuration)command.AppointmentDuration;
        var workdays = command.Workdays
            .Select(d => (DayOfWeek)d)
            .ToList();

        var workSchedule = await workScheduleRepository.FindWorkScheduleByDoctorIdWithTracking(command.DoctorId, ct);

        if (workSchedule is null)
        {
            var createResult = WorkSchedule.Create(
                doctorId: command.DoctorId,
                startTime: command.StartTime,
                endTime: command.EndTime,
                appointmentDuration: appointmentDuration,
                workdays: workdays);

            if (createResult.IsFailure)
                return Result.Failure<WorkScheduleResponse>(createResult.Error);

            workSchedule = createResult.Value;
            await workScheduleRepository.InsertWorkScheduleAsync(workSchedule, ct);
        }
        else
        {
            var updateResult = workSchedule.Update(
                startTime: command.StartTime,
                endTime: command.EndTime,
                appointmentDuration: appointmentDuration,
                workdays: workdays);

            if (updateResult.IsFailure)
                return Result.Failure<WorkScheduleResponse>(updateResult.Error);
        }

        await unitOfWork.SaveChangesAsync(ct);

        return WorkScheduleResponse.FromWorkSchedule(workSchedule);
    }
}