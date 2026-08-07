namespace Domain.Managers;

public interface IManagerRepository
{
    Task InsertManagerAsync(Manager manager, CancellationToken ct);
    
    Task<IReadOnlyList<Manager>> GetAllManagersAsync(CancellationToken ct);
    
    Task<Manager?> FindManagerByUserIdAsync(Guid userId, CancellationToken ct);
}
