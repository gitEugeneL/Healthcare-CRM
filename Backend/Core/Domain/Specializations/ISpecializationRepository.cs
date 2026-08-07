namespace Domain.Specializations;

public interface ISpecializationRepository
{
    Task InsertSpecializationAsync(Specialization specialization, CancellationToken ct);
    
    Task<Specialization?> FindSpecializationByIdAsync(Guid specializationId, CancellationToken ct);
    
    Task<Specialization?> FindSpecializationByNameAsync(string specializationName, CancellationToken ct);

    Task<IReadOnlyCollection<Specialization>> GetSpecializationsAsync(CancellationToken ct);

    Task DeleteSpecializationByIdAsync(Guid specializationId, CancellationToken ct);
}
