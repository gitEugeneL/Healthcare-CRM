using Domain.Patients;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Patients;

internal sealed class PatientRepository(DataContext dataContext) : IPatientRepository
{
    public async Task InsertPatientAsync(Patient patient, CancellationToken ct)
    {
        await dataContext
            .Patients
            .AddAsync(patient, ct);
    }

    public Task DeletePatientAsync(Patient patient, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public async Task<Patient?> FindPatientByIdAsync(Guid patientId, CancellationToken ct)
    {
        return await dataContext
            .Patients
            .Include(p => p.User)
            .Include(p => p.Address)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == patientId, ct);
    }

    public async Task<Patient?> FindPatientByIdForChangeStatusAsync(Guid userId, CancellationToken ct)
    {
        return await dataContext
            .Patients
            .FirstOrDefaultAsync(p => p.UserId == userId, ct);
    }

    public async Task<Patient?> FindPatientByIdWithTrackingAsync(Guid patientId, CancellationToken ct)
    {
        return await dataContext
            .Patients
            .Include(p => p.User)
            .Include(p => p.Address)
            .FirstOrDefaultAsync(p => p.Id == patientId, ct);
    }

    public async Task<(IReadOnlyList<Patient> List, int Count)> GetAllPatientsWithPaginationAsync(
        int pageNumber, 
        int pageSize, 
        Guid? doctorId, 
        CancellationToken ct)
    {
        var query = dataContext
            .Patients
            .Include(p => p.User)
            .Include(p => p.Address)
            .Include(p => p.Appointments)
            .AsSplitQuery()
            .AsNoTracking();

        if (doctorId.HasValue)
            query = query.Where(p => p.Appointments.Any(a => a.DoctorId == doctorId));

        var count = await query.CountAsync(ct);

        var patients = await query
            .OrderByDescending(p => p.Updated)
            .ThenBy(p => p.Created)
            .ThenBy(p => p.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
        
        return (patients, count);
    }

}