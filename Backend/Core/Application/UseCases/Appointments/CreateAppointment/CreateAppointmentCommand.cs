using Domain.Abstractions.Errors;
using MediatR;

namespace Application.UseCases.Appointments.CreateAppointment;

public sealed record CreateAppointmentCommand(
    Guid PatientId,
    Guid DoctorId,
    DateOnly Date,
    TimeOnly StartTime
) : IRequest<Result<AppointmentResponse>>;