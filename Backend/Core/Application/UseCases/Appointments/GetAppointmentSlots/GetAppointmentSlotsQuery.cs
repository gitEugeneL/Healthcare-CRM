using Domain.Abstractions.Errors;
using MediatR;

namespace Application.UseCases.Appointments.GetAppointmentSlots;

public sealed record GetAppointmentSlotsQuery(
    Guid DoctorId,
    DateOnly Date
) : IRequest<Result<AppointmentSlotsResponse>>;