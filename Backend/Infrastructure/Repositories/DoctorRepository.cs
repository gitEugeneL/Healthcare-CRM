using Application.Common.Interfaces;
using Domain.Entities;
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
            .FirstOrDefaultAsync(doctor => doctor.UserId == id, cancellationToken);
    }
}
