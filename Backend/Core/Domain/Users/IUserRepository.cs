namespace Domain.Users;

public interface IUserRepository
{ 
    Task<bool> UserExistsByEmailAsync(string email, CancellationToken ct);

    Task<User?> FindUserByEmailAsync(string email, CancellationToken ct);
    
    Task<User?> FindUserByEmailWithRefreshTokensAsync(string email, CancellationToken ct);
}
