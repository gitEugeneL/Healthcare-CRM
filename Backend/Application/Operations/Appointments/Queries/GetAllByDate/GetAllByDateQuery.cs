using Application.Common.Models;
using MediatR;

namespace Application.Operations.Appointments.Queries.GetAllByDate;

public record GetAllByDateQuery(
    string Date
) : CurrentUser, IRequest<List<AppointmentResponse>>;

