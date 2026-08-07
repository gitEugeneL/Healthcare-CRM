using Domain.Managers;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Managers;

internal sealed class ManagerRepository(DataContext dataContext) : IManagerRepository
{
    public async Task InsertAsync(Manager manager, CancellationToken ct)
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
