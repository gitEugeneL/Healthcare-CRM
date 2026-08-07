namespace Domain.Offices;

public interface IOfficeRepository
{
    Task InsertOfficeAsync(Office office, CancellationToken ct);
    
    Task<Office?> FindOfficeByIdAsync(Guid officeId, CancellationToken ct);

    Task<bool> OfficeExistsByNumberAsync(int officeNumber, CancellationToken ct);
    
    Task<IReadOnlyList<Office>> GetAllOfficesAsync(CancellationToken ct);
}
