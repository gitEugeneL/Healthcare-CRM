using Domain.Entities;

namespace Application.Common.Interfaces;

public interface IManagerRepository
{
    Task<UserManager> CreateManagerAsync(UserManager manager, CancellationToken cancellationToken);

    Task<UserManager> UpdateManagerAsync(UserManager userManager, CancellationToken cancellationToken);

    Task<UserManager?> FindManagerByUserIdAsync(Guid id, CancellationToken cancellationToken);
}
