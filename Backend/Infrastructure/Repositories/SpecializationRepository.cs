using Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

internal class SpecializationRepository(DataContext dataContext) : ISpecializationRepository
{
    public async Task<Specialization> CreateSpecializationAsync(Specialization s, CancellationToken cancellationToken)
    {
        await dataContext.Specializations
            .AddAsync(s, cancellationToken);

        await dataContext.SaveChangesAsync(cancellationToken);
        return s;
    }

    public async Task<Specialization> UpdateSpecializationAsync(Specialization s, CancellationToken cancellationToken)
    {
        dataContext.Specializations.Update(s);
        await dataContext.SaveChangesAsync(cancellationToken);
        return s;
    }

    public async Task<Specialization?> FindSpecializationByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await dataContext.Specializations
            .Include(s => s.UserDoctors)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }
    
    public async Task<Specialization?> FindSpecializationByValueAsync(string value, CancellationToken cancellationToken)
    {
        return await dataContext.Specializations
            .Include(s => s.UserDoctors)
            .FirstOrDefaultAsync(s => s.Value.ToLower().Equals(value.ToLower()), cancellationToken);
    }

    public async Task<IEnumerable<Specialization>> GetSpecializationsAsync(CancellationToken cancellationToken)
    {
        return await dataContext.Specializations
            .Include(s => s.UserDoctors)
            .OrderBy(s => s.Value)
            .ToListAsync(cancellationToken);
    }

    public async Task DeleteSpecializationAsync(Specialization s, CancellationToken cancellationToken)
    {
        dataContext.Specializations.Remove(s);
        await dataContext.SaveChangesAsync(cancellationToken);
    }
}
