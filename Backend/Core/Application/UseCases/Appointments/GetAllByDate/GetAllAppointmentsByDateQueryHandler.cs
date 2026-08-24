using Domain.Abstractions.Errors;
using Domain.Appointments;
using MediatR;

namespace Application.UseCases.Appointments.GetAllByDate;

internal sealed class GetAllAppointmentsByDateQueryHandler(
    IAppointmentRepository appointmentRepository 
    ) : IRequestHandler<GetAllAppointmentsByDateQuery, Result<IReadOnlyList<AppointmentResponse>>>
{
    public async Task<Result<IReadOnlyList<AppointmentResponse>>> Handle(GetAllAppointmentsByDateQuery query, CancellationToken ct)
    {
        return (await appointmentRepository
                .GetAppointmentsByDateAsync(
                    date: query.Date, 
                    patientId: query.PatientId, 
                    doctorId: query.DoctorId, 
                    ct: ct))
            .Select(AppointmentResponse.FromAppointment)
            .ToList();
    }
}
