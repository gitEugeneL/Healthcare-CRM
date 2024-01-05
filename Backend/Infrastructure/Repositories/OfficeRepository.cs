using Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

internal class OfficeRepository(DataContext dataContext) : IOfficeRepository
{
    public async Task<Office> CreateOfficeAsync(Office office, CancellationToken cancellationToken)
    {
        await dataContext.Offices
            .AddAsync(office, cancellationToken);
        await dataContext.SaveChangesAsync(cancellationToken);
        return office;
    }

    public async Task<Office> UpdateOfficeAsync(Office office, CancellationToken cancellationToken)
    {
        dataContext.Offices.Update(office);
        await dataContext.SaveChangesAsync(cancellationToken);
        return office;
    }
    
    public async Task<Office?> FindOfficeByNumberAsync(ushort number, CancellationToken cancellationToken)
    {
        return await dataContext.Offices
            .FirstOrDefaultAsync(o => o.Number == number, cancellationToken);
    }

    public async Task<Office?> FindOfficeByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await dataContext.Offices
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    public async Task<List<Office>> FindOfficesAsync(CancellationToken cancellationToken)
    {
        return await dataContext.Offices.ToListAsync(cancellationToken);
    }
}