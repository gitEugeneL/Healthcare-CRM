using Domain.Abstractions.Errors;
using MediatR;

namespace Application.UseCases.Appointments.ChangeAppointmentStatus;

public sealed record ChangeAppointmentStatusCommand(
    Guid AppointmentId,
    string Status
) : IRequest<Result<AppointmentResponse>>;