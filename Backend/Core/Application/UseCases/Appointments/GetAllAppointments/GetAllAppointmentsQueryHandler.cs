using Application.UseCases.Common.Pagination;
using Domain.Abstractions.Errors;
using Domain.Appointments;
using MediatR;

namespace Application.UseCases.Appointments.GetAllAppointments;

internal sealed class GetAllAppointmentsQueryHandler(
    IAppointmentRepository appointmentRepository 
    ) : IRequestHandler<GetAllAppointmentsQuery, Result<PaginationResult<AppointmentResponse>>>
{
    public async Task<Result<PaginationResult<AppointmentResponse>>> Handle(GetAllAppointmentsQuery query, CancellationToken ct)
    {
        var (appointments, count) = await appointmentRepository.GetAppointmentsWithPaginationAsync(
            pageNumber: query.NormalizedPageNumber,
            pageSize: query.NormalizedPageSize,
            date: query.Date,
            patientId: query.PatientId,
            doctorId: query.DoctorId,
            ct: ct);

        var response = new PaginationResult<AppointmentResponse>(
            Items: appointments.Select(AppointmentResponse.FromAppointment).ToList(),
            TotalItems: count,
            PageNumber: query.NormalizedPageNumber,
            PageSize: query.NormalizedPageSize);

        return response;
    }
}
