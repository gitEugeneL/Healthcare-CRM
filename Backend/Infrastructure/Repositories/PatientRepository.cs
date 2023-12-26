using Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

internal class PatientRepository(DataContext dataContext) : IPatientRepository
{
    public async Task<UserPatient> CreatePatientAsync(UserPatient patient, CancellationToken cancellationToken)
    {
        await dataContext.UserPatients
            .AddAsync(patient, cancellationToken);

        await dataContext.SaveChangesAsync(cancellationToken);
        return patient;
    }

    public async Task<UserPatient> UpdatePatientAsync(UserPatient patient, CancellationToken cancellationToken)
    {
        dataContext.UserPatients.Update(patient);
        await dataContext.SaveChangesAsync(cancellationToken);
        return patient;
    }

    public async Task<UserPatient?> FindPatientByUserIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await dataContext.UserPatients
            .Include(patient => patient.Address)
            .Include(patient => patient.User)
            .FirstOrDefaultAsync(patient => patient.UserId == id, cancellationToken);
    }

    public async Task DeletePatientAsync(UserPatient patient, CancellationToken cancellationToken)
    {
        dataContext.UserPatients.Remove(patient);
        dataContext.Users.Remove(patient.User);
        dataContext.Addresses.Remove(patient.Address);
        await dataContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<(IEnumerable<UserPatient> List, int Count)> GetPatientsWithPaginationAsync(
        CancellationToken cancellationToken, int pageNumber, int pageSize)
    {
        var query = dataContext.UserPatients
            .Include(p => p.Address)
            .Include(p => p.User)
            .AsQueryable();

        var count = await query.CountAsync(cancellationToken);

        var patients = await query
            .Skip(pageSize * (pageNumber - 1))
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (patients, count);
    }
}