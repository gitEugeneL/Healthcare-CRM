using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Users;

internal class UserRepository(DataContext dataContext) : IUserRepository
{
    public async Task<bool> UserExistsByEmailAsync(string email, CancellationToken ct)
    {
        return await dataContext
            .Users
            .AsNoTracking()
            .AnyAsync(u => u.Email == email, ct);
    }

    public async Task<User?> FindUserByEmailAsync(string email, CancellationToken ct)
    {
        return await dataContext
            .Users
            .FirstOrDefaultAsync(u => u.Email == email, ct);
    }

    public async Task<User?> FindUserByEmailWithRefreshTokensAsync(string email, CancellationToken ct)
    {
        return await dataContext
            .Users
            .Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.Email == email, ct);
    }
}