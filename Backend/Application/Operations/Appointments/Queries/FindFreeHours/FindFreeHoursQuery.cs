using MediatR;

namespace Application.Operations.Appointments.Queries.FindFreeHours;

public sealed record FindFreeHoursQuery(
    Guid UserDoctorId,
    string Date
) : IRequest<FreeHoursResponse>;
