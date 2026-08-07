using Domain.Offices;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Offices;

internal sealed class OfficeRepository(DataContext dataContext) : IOfficeRepository
{
    public async Task InsertOfficeAsync(Office office, CancellationToken ct)
    {
        await dataContext
            .Offices
            .AddAsync(office, ct);
    }

    public async Task<bool> OfficeExistsByNumberAsync(int officeNumber, CancellationToken ct)
    {
        return await dataContext
            .Offices
            .AnyAsync(o => o.Number == officeNumber, ct);
    }

    public async Task<IReadOnlyList<Office>> GetAllOfficesAsync(CancellationToken ct)
    {
        return await dataContext
            .Offices
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task<Office?> FindOfficeByIdAsync(Guid officeId, CancellationToken ct)
    {
       return await dataContext
           .Offices
           .FirstOrDefaultAsync(o => o.Id == officeId, ct);
    }
}