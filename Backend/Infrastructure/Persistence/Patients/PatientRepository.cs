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
    
    public Task<(IReadOnlyList<Patient> List, int Count)> GetPatientsWithPaginationAsync(
        int pageNumber, 
        int pageSize, 
        Guid? doctorId, 
        Guid? specializationId,
        CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}