using Application.Common.Models;
using MediatR;

namespace Application.Operations.Appointments.Commands.FinaliseAppointment;

public sealed record FinaliseAppointmentCommand(Guid AppointmentId) : CurrentUser, IRequest<AppointmentResponse>;
