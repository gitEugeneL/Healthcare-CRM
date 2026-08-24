using Domain.Abstractions.Errors;
using MediatR;

namespace Application.UseCases.Appointments.GetAllByDate;

public sealed record GetAllAppointmentsByDateQuery(
    Guid? DoctorId,
    Guid? PatientId,
    DateOnly Date
) : IRequest<Result<IReadOnlyList<AppointmentResponse>>>; 
