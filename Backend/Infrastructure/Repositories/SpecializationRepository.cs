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

    public Task<Specialization> UpdateSpecializationAsync(Specialization s, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<Specialization?> FindSpecializationByValueAsync(string value, CancellationToken cancellationToken)
    {
        return await dataContext.Specializations
            .FirstOrDefaultAsync(s => s.Value.ToLower().Equals(value.ToLower()), cancellationToken);
    }

    public Task<IEnumerable<Specialization>> GetSpecializationsAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}