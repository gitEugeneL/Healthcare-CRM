namespace Domain.Patients;

public interface IPatientRepository
{
    Task InsertPatientAsync(Patient patient, CancellationToken ct);
    
    Task DeletePatientAsync(Patient patient, CancellationToken ct);
    
    Task<Patient?> FindPatientByIdAsync(Guid patientId, CancellationToken ct);

    Task<Patient?> FindPatientByIdWithTrackingAsync(Guid patientId, CancellationToken ct);
    
    Task<(IReadOnlyList<Patient> List, int Count)> GetAllPatientsWithPaginationAsync(
        int pageNumber, 
        int pageSize,
        Guid? doctorId,
        CancellationToken ct);
}
