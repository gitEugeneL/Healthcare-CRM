using Domain.Specializations;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Specializations;

internal sealed class SpecializationRepository(DataContext dataContext) : ISpecializationRepository
{
    public async Task InsertSpecializationAsync(Specialization specialization, CancellationToken ct)
    {
        await dataContext
            .Specializations
            .AddAsync(specialization, ct);
    }

    public async Task<IReadOnlyList<Specialization>> GetAllSpecializationsWithDoctorsAsync(CancellationToken ct)
    {
        return await dataContext
            .Specializations
            .Include(s => s.Doctors)
            .ThenInclude(d => d.User)
            .OrderByDescending(s => s.Doctors.Count)
            .AsNoTracking()
            .ToListAsync(ct);
    }
    
    
    public async Task<Specialization?> FindSpecializationByIdWithTrackingAsync(Guid specializationId, CancellationToken ct)
    {
        return await dataContext
            .Specializations
            .FirstOrDefaultAsync(s => s.Id == specializationId, ct);
    }

    public async Task<Specialization?> FindSpecializationByIdWithDoctorsAndWithTrackingAsync(Guid specializationId, CancellationToken ct)
    {
        return await dataContext
            .Specializations
            .Include(s => s.Doctors)
            .ThenInclude(d => d.User)
            .FirstOrDefaultAsync(s => s.Id == specializationId, ct);   
    }

    public async Task<Specialization?> FindSpecializationByIdWithDoctorsAndAsync(Guid specializationId, CancellationToken ct)
    {
        return await dataContext
            .Specializations
            .Include(s => s.Doctors)
            .ThenInclude(d => d.User)
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == specializationId, ct);   
    }
    
    public async Task<bool> SpecializationExistsByNameAsync(string specializationName, CancellationToken ct)
    {
        return await dataContext
            .Specializations
            .AnyAsync(s => s.Name == specializationName.Trim().ToUpperInvariant(), ct);
    }
    
    public async Task<bool?> IsSpecializationEmptyByIdAsync(Guid specializationId, CancellationToken ct)
    {
        var hasDoctors = await dataContext
            .Specializations
            .AsNoTracking()
            .Where(s => s.Id == specializationId)
            .Select(s => (bool?)s.Doctors.Any())
            .SingleOrDefaultAsync(ct);

        return !hasDoctors;
    }
    
    public async Task DeleteSpecializationByIdAsync(Guid specializationId, CancellationToken ct)
    {
        await dataContext
            .Specializations
            .Where(s => s.Id == specializationId)
            .ExecuteDeleteAsync(ct);
    }
}