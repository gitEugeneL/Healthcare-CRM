using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

internal class DoctorRepository(DataContext dataContext) : IDoctorRepository
{
    public async Task<UserDoctor> CreateDoctorAsync(UserDoctor doctor, CancellationToken cancellationToken)
    {
        await dataContext.UserDoctors
            .AddAsync(doctor, cancellationToken);

        await dataContext.SaveChangesAsync(cancellationToken);
        return doctor;
    }

    public async Task<UserDoctor> UpdateDoctorAsync(UserDoctor doctor, CancellationToken cancellationToken)
    {
        dataContext.UserDoctors.Update(doctor);
        await dataContext.SaveChangesAsync(cancellationToken);
        return doctor;
    }

    public async Task<UserDoctor?> FindDoctorByUserIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await dataContext.UserDoctors
            .Include(doctor => doctor.User)
            .Include(doctor => doctor.Specializations)
            .FirstOrDefaultAsync(doctor => doctor.UserId == id, cancellationToken);
    }

    public async Task<(IEnumerable<UserDoctor> List, int Count)> GetDoctorsWithPaginationAsync(
        CancellationToken cancellationToken, int pageNumber, int pageSize, Guid? specializationId = null)
    {
        var query = dataContext.UserDoctors
            .Include(d => d.User)
            .Include(d => d.Specializations)
            .Where(doctor => doctor.Status == Status.Active)
            .AsQueryable();
        
        query = specializationId.HasValue
            ? query
                .Where(doctor => doctor.Specializations
                    .Any(s => s.Id == specializationId))
            : query;

        var count = await query.CountAsync(cancellationToken);

        var doctors = await query
            .Skip(pageSize * (pageNumber - 1))
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (doctors, count);
    }
}
