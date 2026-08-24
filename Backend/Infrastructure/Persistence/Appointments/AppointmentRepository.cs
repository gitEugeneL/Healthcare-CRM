using Domain.Appointments;
using Domain.Doctors;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Appointments;

internal sealed class AppointmentRepository(DataContext dataContext) : IAppointmentRepository
{
    public async Task InsertAppointmentAsync(Appointment appointment, CancellationToken ct)
    {
        await dataContext
            .Appointments
            .AddAsync(appointment, ct);
    }
    
    public async Task<List<(TimeOnly Start, TimeOnly End)>> GetAvailableSlots(
        Doctor doctor, 
        DateOnly date, 
        CancellationToken ct)
    {
        if (doctor.Status != DoctorStatus.Active || doctor.WorkSchedule is null)
            return [];

        var busyTime = await dataContext
            .Appointments
            .Include(d => d.Doctor)
            .Where(a =>
                a.DoctorId == doctor.Id
                && a.Date == date
                && a.Status != AppointmentStatus.Canceled
                && doctor.Status == DoctorStatus.Active)
            .Select(a => a.StartTime)
            .ToHashSetAsync(ct);
        
        var duration = (int) doctor.WorkSchedule.AppointmentDuration;
        var freeTime = new List<(TimeOnly Start, TimeOnly End)>();
        
        var slotStart = doctor.WorkSchedule.StartTime;
        while (slotStart.AddMinutes(duration) <= doctor.WorkSchedule.EndTime)
        {
            if (!busyTime.Contains(slotStart))
                freeTime.Add((slotStart, slotStart.AddMinutes(duration)));

            slotStart = slotStart.AddMinutes(duration);
        }

        return freeTime;
    }

    public async Task<bool> IsPatientAlreadyBookedAsync(Guid doctorId, Guid patientId, DateOnly date, CancellationToken ct)
    {
        return await dataContext
            .Appointments
            .AnyAsync(a =>
                a.DoctorId == doctorId
                && a.PatientId == patientId
                && a.Date == date
                && a.Status != AppointmentStatus.Canceled, ct);
    }

    public async Task<Appointment?> FindAppointmentByIdWithTrackingAsync(Guid appointmentId, CancellationToken ct)
    {
        return await dataContext
            .Appointments
            .FirstOrDefaultAsync(a => a.Id == appointmentId, ct);
    }

    public async Task<IReadOnlyList<Appointment>> GetAppointmentsByDateAsync(
        DateOnly date,
        Guid? doctorId, 
        Guid? patientId,
        CancellationToken ct)
    {
        var result = await dataContext
            .Appointments
            .OrderByDescending(a => a.StartTime)
            .Where(a => a.Date == date)
            .ToListAsync(ct);

        if (doctorId.HasValue)
        {
            result = result
                .Where(a => a.DoctorId == doctorId)
                .ToList();
        }

        if (patientId.HasValue)
        {
            result = result
                .Where(a => a.PatientId == patientId)
                .ToList();
        }
        
        return result;
    }
}
