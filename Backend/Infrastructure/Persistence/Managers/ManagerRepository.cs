using Domain.Managers;
using Microsoft.EntityFrameworkCore;
using Persistence.Persistence;

namespace Persistence.Managers;

internal sealed class ManagerRepository(DataContext dataContext) : IManagerRepository
{
    public async Task InsertManagerAsync(Manager manager, CancellationToken ct)
    {
        await dataContext
            .Managers
            .AddAsync(manager, ct);
    }

    public async Task<IReadOnlyList<Manager>> GetAllManagersAsync(CancellationToken ct)
    {
        return await dataContext
            .Managers
            .Include(manager => manager.User)
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task<Manager?> FindManagerByUserIdAsync(Guid userId, CancellationToken ct)
    {
        return await dataContext
            .Managers
            .Include(manager => manager.User)
            .FirstOrDefaultAsync(manager => manager.UserId == userId, ct);
    }
}
