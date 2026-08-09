using Domain.Doctors;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Doctors;

internal sealed class DoctorRepository(DataContext dataContext) : IDoctorRepository
{
    public async Task InsertDoctorAsync(Doctor doctor, CancellationToken ct)
    {
        await dataContext
            .Doctors
            .AddAsync(doctor, ct);
    }

    public async Task<bool> DoctorExistsAsync(Guid doctorId, CancellationToken ct)
    {
        return await dataContext
            .Doctors
            .AnyAsync(d => d.Id == doctorId, ct);
    }

    public async Task<Doctor?> FindDoctorByIdAsync(Guid doctorId, CancellationToken ct)
    {
        return await dataContext
            .Doctors
            .Include(d => d.User)
            .Include(d => d.Specializations)
            .Include(d => d.WorkSchedule)
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.Id == doctorId, ct);
    }

    public async Task<Doctor?> FindDoctorByIdWithTrackingAsync(Guid doctorId, CancellationToken ct)
    {
        return await dataContext
            .Doctors
            .Include(d => d.User)
            .FirstOrDefaultAsync(d => d.Id == doctorId, ct);
    }

    public async Task<Doctor?> FindDoctorForChangeStatus(Guid doctorId, CancellationToken ct)
    {
        return await dataContext
            .Doctors
            .Include(d => d.Specializations)
            .Include(d => d.WorkSchedule)
            .FirstOrDefaultAsync(d => d.Id == doctorId, ct);
    }

    public Task<(IReadOnlyList<Doctor> List, int Count)> GetDoctorsWithPaginationAsync(
        int pageNumber, 
        int pageSize, 
        Guid? specializationId, 
        DoctorStatus? status,
        CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}
