using Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

internal class ManagerRepository(DataContext dataContext) : IManagerRepository
{
    public async Task<UserManager> CreateManagerAsync(UserManager manager, CancellationToken cancellationToken)
    {
        await dataContext.UserManagers
            .AddAsync(manager, cancellationToken);

        await dataContext.SaveChangesAsync(cancellationToken);
        return manager;
    }

    public async Task<UserManager> UpdateManagerAsync(UserManager manager, CancellationToken cancellationToken)
    {
        dataContext.UserManagers.Update(manager);
        await dataContext.SaveChangesAsync(cancellationToken);
        return manager;
    }

    public async Task<UserManager?> FindManagerByUserIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await dataContext.UserManagers
            .Include(manager => manager.User)
            .FirstOrDefaultAsync(manager => manager.UserId == id, cancellationToken);
    }
}
