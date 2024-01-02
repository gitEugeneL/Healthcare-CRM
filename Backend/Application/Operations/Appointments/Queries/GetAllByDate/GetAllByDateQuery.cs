using Application.Common.Models;
using Application.Operations.Appointments.Validations;
using MediatR;

namespace Application.Operations.Appointments.Queries.GetAllByDate;

public record GetAllByDateQuery(
    [DateValidation]
    string Date
) : CurrentUser, IRequest<List<AppointmentResponse>>;
