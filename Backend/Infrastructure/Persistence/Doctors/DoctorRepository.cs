using Domain.Doctors;
using Persistence.Database;

namespace Persistence.Doctors;

internal sealed class DoctorRepository(DataContext dataContext) : IDoctorRepository
{
    // public async Task<UserDoctor> CreateDoctorAsync(UserDoctor doctor, CancellationToken cancellationToken)
    // {
    //     await dataContext.UserDoctors
    //         .AddAsync(doctor, cancellationToken);
    //
    //     await dataContext.SaveChangesAsync(cancellationToken);
    //     return doctor;
    // }
    //
    // public async Task<UserDoctor> UpdateDoctorAsync(UserDoctor doctor, CancellationToken cancellationToken)
    // {
    //     dataContext.UserDoctors.Update(doctor);
    //     await dataContext.SaveChangesAsync(cancellationToken);
    //     return doctor;
    // }
    //
    // public async Task<UserDoctor?> FindDoctorByUserIdAsync(Guid id, CancellationToken cancellationToken)
    // {
    //     return await dataContext.UserDoctors
    //         .Include(doctor => doctor.User)
    //         .Include(doctor => doctor.Specializations)
    //         .Include(doctor => doctor.AppointmentSettings)
    //         .FirstOrDefaultAsync(doctor => doctor.UserId == id, cancellationToken);
    // }
    //
    // public async Task<(IEnumerable<UserDoctor> List, int Count)> GetDoctorsWithPaginationAsync(
    //     CancellationToken cancellationToken, int pageNumber, int pageSize, Guid? specializationId = null)
    // {
    //     var query = dataContext.UserDoctors
    //         .Include(d => d.User)
    //         .Include(d => d.Specializations)
    //         .Where(doctor => doctor.Status == Status.Active);
    //     
    //     query = specializationId.HasValue
    //         ? query
    //             .Where(doctor => doctor.Specializations
    //                 .Any(s => s.Id == specializationId))
    //         : query;
    //
    //     var count = await query.CountAsync(cancellationToken);
    //
    //     var doctors = await query
    //         .Skip(pageSize * (pageNumber - 1))
    //         .Take(pageSize)
    //         .ToListAsync(cancellationToken);
    //
    //     return (doctors, count);
    // }
    public Task InsertDoctorAsync(Doctor doctor, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task<Doctor?> FindDoctorByUserIdAsync(Guid userId, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task<(IReadOnlyList<Doctor> List, int Count)> GetDoctorsWithPaginationAsync(
        int pageNumber, 
        int pageSize, 
        Guid? specializationId, 
        CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}
