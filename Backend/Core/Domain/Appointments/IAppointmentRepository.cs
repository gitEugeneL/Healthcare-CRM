using Domain.Doctors;

namespace Domain.Appointments;

public interface IAppointmentRepository
{
    Task InsertAppointmentAsync(Appointment appointment, CancellationToken ct);

    public Task<List<(TimeOnly Start, TimeOnly End)>> GetAvailableSlots(
        Doctor doctor,
        DateOnly date,
        CancellationToken ct);
    
    Task<bool> IsPatientAlreadyBookedAsync(Guid doctorId, Guid patientId, DateOnly date, CancellationToken ct);
    
    Task<Appointment?> FindAppointmentByIdWithTrackingAsync(Guid appointmentId, CancellationToken ct);

    Task<(IReadOnlyList<Appointment> List, int Count)> GetAppointmentsWithPaginationAsync(
        int pageNumber,
        int pageSize,
        DateOnly? date,
        Guid? doctorId,
        Guid? patientId,
        CancellationToken ct);
}
