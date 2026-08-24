using Domain.Abstractions;
using Domain.Abstractions.Errors;
using Domain.Appointments;
using MediatR;

namespace Application.UseCases.Appointments.ChangeAppointmentStatus;

internal sealed class ChangeAppointmentStatusCommandHandler(
    IAppointmentRepository appointmentRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<ChangeAppointmentStatusCommand, Result<AppointmentResponse>>
{
    public async Task<Result<AppointmentResponse>> Handle(ChangeAppointmentStatusCommand command, CancellationToken ct)
    { 
        if (!Enum.TryParse<AppointmentStatus>(command.Status, ignoreCase: true, out var status) || !Enum.IsDefined(status))
            return Result.Failure<AppointmentResponse>(AppointmentErrors.InvalidStatus(command.Status));
        
        var appointment = await appointmentRepository.FindAppointmentByIdWithTrackingAsync(command.AppointmentId, ct);
        if (appointment is null)
            return Result.Failure<AppointmentResponse>(AppointmentErrors.NotFound(command.AppointmentId));

        Result result = status switch
        {
            AppointmentStatus.Planned => appointment.Initialize(),
            AppointmentStatus.Canceled => appointment.Cancel(),
            AppointmentStatus.Confirmed => appointment.Confirm(),
            AppointmentStatus.InProgress => appointment.Start(),
            AppointmentStatus.Completed => appointment.Complete()
        };

        if (result.IsFailure)
            return Result.Failure<AppointmentResponse>(result.Error);

        await unitOfWork.SaveChangesAsync(ct);
        
        return AppointmentResponse.FromAppointment(appointment);
    }
}