using Domain.Entities;

namespace Application.Common.Interfaces;

public interface ISpecializationRepository
{
    Task<Specialization> CreateSpecializationAsync(Specialization specialization, CancellationToken cancellationToken);

    Task<Specialization> UpdateSpecializationAsync(Specialization specialization, CancellationToken cancellationToken);

    Task<Specialization?> FindSpecializationByIdAsync(Guid id, CancellationToken cancellationToken);
    
    Task<Specialization?> FindSpecializationByValueAsync(string value, CancellationToken cancellationToken);

    Task<IEnumerable<Specialization>> GetSpecializationsAsync(CancellationToken cancellationToken);

    Task DeleteSpecializationAsync(Specialization specialization, CancellationToken cancellationToken);
}
