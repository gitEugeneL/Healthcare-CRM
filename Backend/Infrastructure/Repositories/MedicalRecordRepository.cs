using Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class MedicalRecordRepository(DataContext dataContext) : IMedicalRecordRepository
{
    public async Task<MedicalRecord> CreateMedicalRecordAsync(MedicalRecord medicalRecord, 
        CancellationToken cancellationToken)
    {
        await dataContext.MedicalRecords
            .AddAsync(medicalRecord, cancellationToken);
        await dataContext.SaveChangesAsync(cancellationToken);
        return medicalRecord;
    }

    public async Task<MedicalRecord> UpdateMedicalRecordAsync(MedicalRecord medicalRecord,
        CancellationToken cancellationToken)
    {
        dataContext.MedicalRecords
            .Update(medicalRecord);
        await dataContext.SaveChangesAsync(cancellationToken);
        return medicalRecord;
    }

    public async Task<MedicalRecord?> FindMedicalRecordByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await dataContext.MedicalRecords
            .Include(mr => mr.UserPatient)
            .Include(mr => mr.UserDoctor)
            .Include(mr => mr.Appointment)
            .FirstOrDefaultAsync(mr => mr.Id == id, cancellationToken);
    }
    
    public async Task<MedicalRecord?> FindMedicalRecordByAppointmentIdAsync(Guid id, 
        CancellationToken cancellationToken)
    {
        return await dataContext.MedicalRecords
            .FirstOrDefaultAsync(mr => mr.AppointmentId == id, cancellationToken);
    }

    public async Task<(IEnumerable<MedicalRecord> List, int Count)> GetMedicalRecordsForPatientWithPaginationAsync(
        CancellationToken cancellationToken,
        int pageNumber,
        int pageSize,
        Guid patientId,
        bool sortByDate = false,
        bool sortOrderAsc = true
    )
    {
        var query = dataContext.MedicalRecords
            .Include(mr => mr.UserDoctor)
            .Include(mr => mr.UserPatient)
            .Include(mr => mr.Appointment)
            .Where(mr => mr.UserPatient.UserId == patientId)
            .AsQueryable();

        if (sortByDate && sortOrderAsc)
            query = query.OrderBy(mr => mr.Created);

        if (sortByDate && !sortOrderAsc)
            query = query.OrderByDescending(mr => mr.Created);

        var count = await query.CountAsync(cancellationToken);

        var medicalRecords = await query
            .Skip(pageSize * (pageNumber - 1))
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (medicalRecords, count);
    }
    
    public async Task<(IEnumerable<MedicalRecord> List, int Count)> GetMedicalRecordsForDoctorWithPaginationAsync(
        CancellationToken cancellationToken,
        int pageNumber,
        int pageSize,
        Guid doctorId,
        Guid? patientId = null,
        bool sortByDate = false,
        bool sortOrderAsc = true
    )
    {
        var query = dataContext.MedicalRecords
            .Include(mr => mr.UserDoctor)
            .Include(mr => mr.UserPatient)
            .Include(mr => mr.Appointment)
            .Where(mr => mr.UserDoctor.UserId == doctorId)
            .AsQueryable();

        if (patientId is not null)
            query = query.Where(mr => mr.UserPatient.UserId == patientId);
        
        if (sortByDate && sortOrderAsc)
            query = query.OrderBy(mr => mr.Created);

        if (sortByDate && !sortOrderAsc)
            query = query.OrderByDescending(mr => mr.Created);

        var count = await query.CountAsync(cancellationToken);

        var medicalRecords = await query
            .Skip(pageSize * (pageNumber - 1))
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (medicalRecords, count);
    }
}
