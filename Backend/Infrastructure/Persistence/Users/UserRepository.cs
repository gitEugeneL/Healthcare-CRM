using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Persistence.Persistence;

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
    
    // public async Task<User?> FindUserByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
    // {
    //     return await dataContext.Users
    //         .Include(user => user.RefreshTokens)
    //         .FirstOrDefaultAsync(user => user.RefreshTokens
    //                 .Any(rt => rt.Token == refreshToken), cancellationToken);
    // }
}
