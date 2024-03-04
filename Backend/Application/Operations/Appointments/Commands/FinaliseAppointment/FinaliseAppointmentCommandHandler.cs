using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Operations.Appointments.Commands.FinaliseAppointment;

public class FinaliseAppointmentCommandHandler(IAppointmentRepository appointmentRepository) 
    : IRequestHandler<FinaliseAppointmentCommand, AppointmentResponse>
{
    public async Task<AppointmentResponse> Handle(FinaliseAppointmentCommand request, CancellationToken cancellationToken)
    {
        var appointment = await appointmentRepository.FindAppointmentByIdAsync(request.AppointmentId, cancellationToken)
                          ?? throw new NotFoundException(nameof(Appointment), request.AppointmentId);
        
        if (appointment.UserDoctor.UserId != request.GetCurrentUserId())
            throw new AccessDeniedException(nameof(User), request.GetCurrentUserId());

        if (appointment.Date != DateOnly.FromDateTime(DateTime.Now))
            throw new UnprocessableException(
                $"Appointment can be finalise {appointment.Date.ToString("yyyy-M-d")}");
        
        appointment.IsCompleted = true;
        var updatedAppointment = await appointmentRepository.UpdateAppointmentAsync(appointment, cancellationToken);
        return new AppointmentResponse()
            .ToAppointmentResponse(appointment);
    }
}
