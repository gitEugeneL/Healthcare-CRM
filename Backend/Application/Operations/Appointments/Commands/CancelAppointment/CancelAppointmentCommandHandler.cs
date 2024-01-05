using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Operations.Appointments.Commands.CancelAppointment;

public class CancelAppointmentCommandHandler(IAppointmentRepository appointmentRepository)
    : IRequestHandler<CancelAppointmentCommand, AppointmentResponse>
{
    public async Task<AppointmentResponse> Handle(CancelAppointmentCommand request, CancellationToken cancellationToken)
    {
        var appointment = await appointmentRepository.FindAppointmentByIdAsync(request.AppointmentId, cancellationToken)
                          ?? throw new NotFoundException(nameof(Appointment), request.AppointmentId);
        
        if (appointment.UserDoctor.UserId != request.GetCurrentUserId())
            throw new AccessDeniedException(nameof(User), request.GetCurrentUserId());

        appointment.IsCanceled = true;
        var updatedAppointment = await appointmentRepository.UpdateAppointmentAsync(appointment, cancellationToken);
        return new AppointmentResponse()
            .ToAppointmentResponse(appointment);
    }
}
