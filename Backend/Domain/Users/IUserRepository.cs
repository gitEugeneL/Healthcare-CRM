namespace Domain.Users;

public interface IUserRepository
{ 
    Task<bool> UserExistsByEmailAsync(string email, CancellationToken ct);
    
    // Task<User?> FindUserByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken);
}
