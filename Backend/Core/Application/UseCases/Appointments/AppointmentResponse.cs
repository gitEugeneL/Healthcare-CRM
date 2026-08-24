using Domain.Appointments;

namespace Application.UseCases.Appointments;

public sealed record AppointmentResponse(
    Guid AppointmentId,
    Guid PatientId,
    Guid DoctorId,
    DateOnly Date,
    TimeOnly StartTime,
    TimeOnly EndTime,
    string Status)
{
    public static AppointmentResponse FromAppointment(Appointment appointment)
    {
        return new AppointmentResponse(
            appointment.Id, 
            appointment.PatientId, 
            appointment.DoctorId, 
            appointment.Date,
            appointment.StartTime, 
            appointment.EndTime, 
            appointment.Status.ToString());
    }
}