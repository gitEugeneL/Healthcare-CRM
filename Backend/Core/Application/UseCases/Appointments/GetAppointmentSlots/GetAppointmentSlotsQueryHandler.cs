using Domain.Abstractions.Errors;
using Domain.Appointments;
using Domain.Doctors;
using Domain.WorkSchedules;
using MediatR;

namespace Application.UseCases.Appointments.GetAppointmentSlots;

internal sealed class GetAppointmentSlotsQueryHandler(
    IDoctorRepository doctorRepository,
    IAppointmentRepository appointmentRepository
) : IRequestHandler<GetAppointmentSlotsQuery, Result<AppointmentSlotsResponse>>
{
    public async Task<Result<AppointmentSlotsResponse>> Handle(GetAppointmentSlotsQuery query, CancellationToken ct)
    {
        var doctor = await doctorRepository.FindActiveDoctorByIdAsync(query.DoctorId, ct);
        if (doctor?.WorkSchedule is null)
        {
            return Result.Failure<AppointmentSlotsResponse>(DoctorErrors.NotFound(query.DoctorId));
        }
        
        if (!doctor.WorkSchedule.IsAvailableDay(query.Date.DayOfWeek))
        {
            return Result.Failure<AppointmentSlotsResponse>(WorkScheduleErrors.UnavailableWorkday(query.Date.DayOfWeek));
        }
        
        var slots = (await appointmentRepository.GetAvailableSlots(doctor, query.Date, ct))
            .Select(s => new TimeSlot(s.Start, s.End))
            .ToList();
        
        return new AppointmentSlotsResponse(doctor.Id, query.Date, slots);
    }
}