using Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

internal class UserRepository(DataContext dataContext) : IUserRepository
{
    public async Task<User> CreateUserAsync(User user, CancellationToken cancellationToken)
    {
        await dataContext.Users
            .AddAsync(user, cancellationToken);
        await dataContext
            .SaveChangesAsync(cancellationToken);
        return user;
    }

    public async Task<User?> FindUserByEmailAsync(string email, CancellationToken cancellationToken)
    {
        return await dataContext.Users
            .Include(user => user.RefreshTokens)
            .FirstOrDefaultAsync(user => user.Email == email, cancellationToken);
    }

    public async Task<User?> FindUserByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
    {
        return await dataContext.Users
            .Include(user => user.RefreshTokens)
            .FirstOrDefaultAsync(user => user.RefreshTokens
                    .Any(rt => rt.Token == refreshToken), cancellationToken);
    }
    
    public async Task UpdateUserAsync(User user, CancellationToken cancellationToken)
    {
        dataContext.Users.Update(user);
        await dataContext.SaveChangesAsync(cancellationToken);
    }
}
