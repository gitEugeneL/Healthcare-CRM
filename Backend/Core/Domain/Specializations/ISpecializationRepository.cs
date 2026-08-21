namespace Domain.Specializations;

public interface ISpecializationRepository
{
    Task InsertSpecializationAsync(Specialization specialization, CancellationToken ct);
    
    Task<IReadOnlyList<Specialization>> GetAllSpecializationsWithDoctorsAsync(CancellationToken ct);
    
    Task<Specialization?> FindSpecializationByIdWithTrackingAsync(Guid specializationId, CancellationToken ct);

    Task<Specialization?> FindSpecializationByIdWithDoctorsAndWithTrackingAsync(
        Guid specializationId, 
        CancellationToken ct);

    Task<Specialization?> FindSpecializationByIdWithDoctorsAndAsync(Guid specializationId, CancellationToken ct);
    
    Task<bool> SpecializationExistsByNameAsync(string specializationName, CancellationToken ct);

    Task<bool?> IsSpecializationEmptyByIdAsync(Guid specializationId, CancellationToken ct);
    
    Task DeleteSpecializationByIdAsync(Guid specializationId, CancellationToken ct);
}
