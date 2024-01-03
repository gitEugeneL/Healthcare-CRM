using Application.Common.Models;
using MediatR;

namespace Application.Operations.Appointments.Commands.CancelAppointment;

public sealed record CancelAppointmentCommand(Guid AppointmentId) : CurrentUser, IRequest<AppointmentResponse>;
