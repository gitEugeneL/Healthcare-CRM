using Application.UseCases.Common.Pagination;
using Domain.Abstractions.Errors;
using MediatR;

namespace Application.UseCases.Appointments.GetAllAppointments;

public sealed record GetAllAppointmentsQuery(
    int? PageNumber, 
    int? PageSize,
    DateOnly? Date,
    Guid? DoctorId,
    Guid? PatientId
) : PaginationQuery(PageNumber, PageSize), IRequest<Result<PaginationResult<AppointmentResponse>>>; 
