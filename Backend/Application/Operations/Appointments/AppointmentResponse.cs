using Domain.Entities;

namespace Application.Operations.Appointments;

public sealed record AppointmentResponse
{
    public Guid AppointmentId { get; set; }
    public Guid UserPatientId { get; set; }
    public Guid UserDoctorId { get; set; }
    public string Date { get; set; } = string.Empty;
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public bool IsCompleted { get; set; }
    public bool IsCanceled { get; set; }

    public AppointmentResponse ToAppointmentResponse(Appointment appointment)
    {
        AppointmentId = appointment.Id;
        UserPatientId = appointment.UserPatient.UserId;
        UserDoctorId = appointment.UserDoctor.UserId;
        Date = appointment.Date.ToString("yyyy-MM-dd");
        StartTime = appointment.StartTime;
        EndTime = appointment.EndTime;
        IsCompleted = appointment.IsCompleted;
        IsCanceled = appointment.IsCanceled;

        return this;
    }
}
