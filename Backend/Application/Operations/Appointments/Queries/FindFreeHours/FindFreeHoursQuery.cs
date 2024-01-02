using Application.Operations.Appointments.Validations;
using MediatR;

namespace Application.Operations.Appointments.Queries.FindFreeHours;

public sealed record FindFreeHoursQuery(
    Guid UserDoctorId,
    [DateValidation] 
    string Date
) : IRequest<FreeHoursResponse>;
