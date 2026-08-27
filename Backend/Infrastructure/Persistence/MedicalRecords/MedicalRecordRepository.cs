using Domain.MedicalRecords;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.MedicalRecords;

internal sealed class MedicalRecordRepository(DataContext dataContext) : IMedicalRecordRepository
{
    public async Task InsertMedicalRecordAsync(MedicalRecord medicalRecord, CancellationToken ct)
    {
        await dataContext
            .MedicalRecords
            .AddAsync(medicalRecord, ct);
    }

    public async Task<bool> IsMedicalRecordByAppointmentIdExistsAsync(Guid appointmentId, CancellationToken ct)
    {
        return await dataContext
            .MedicalRecords
            .AnyAsync(mr => mr.AppointmentId == appointmentId, ct);
    }

    public async Task<MedicalRecord?> FindMedicalRecordByIdWithTrackingAsync(Guid medicalRecordId, CancellationToken ct)
    {
        return await dataContext
            .MedicalRecords
            .Include(mr => mr.Appointment)
            .FirstOrDefaultAsync(mr => mr.Id == medicalRecordId, ct);
    }

    public async Task<(IReadOnlyList<MedicalRecord> List, int Count)> GetAllMedicalRecordsWithPaginationAsync(
        int pageNumber, 
        int pageSize, 
        Guid? doctorId, 
        Guid? patientId,
        CancellationToken ct)
    {
        var query = dataContext
            .MedicalRecords
            .Include(mr => mr.Appointment)
            .AsSplitQuery()
            .AsNoTracking();

        if (doctorId.HasValue)
            query = query.Where(mr => mr.Appointment.DoctorId == doctorId);
        
        if (patientId.HasValue)
            query = query.Where(mr => mr.Appointment.PatientId == patientId);
            
        var count = await query.CountAsync(ct);
        
        var medicalRecords = await query
            .OrderByDescending(mr => mr.Created)
            .ThenBy(mr => mr.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (medicalRecords, count);
    }
}
