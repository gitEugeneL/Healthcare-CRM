using Domain.Entities;

namespace Application.Common.Interfaces;

public interface IOfficeRepository
{
    Task<Office> CreateOfficeAsync(Office office, CancellationToken cancellationToken);
    
    Task<Office> UpdateOfficeAsync(Office office, CancellationToken cancellationToken);

    Task<Office?> FindOfficeByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<Office?> FindOfficeByNumberAsync(int number, CancellationToken cancellationToken);

    Task<List<Office>> FindOfficesAsync(CancellationToken cancellationToken);
}
