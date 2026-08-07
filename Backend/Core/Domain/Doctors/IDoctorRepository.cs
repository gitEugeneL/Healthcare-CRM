namespace Domain.Doctors;

public interface IDoctorRepository
{
    Task InsertDoctorAsync(Doctor doctor, CancellationToken ct);
    
    Task<Doctor?> FindDoctorByUserIdAsync(Guid userId, CancellationToken ct);

    Task<(IReadOnlyList<Doctor> List, int Count)> GetDoctorsWithPaginationAsync(
        int pageNumber, 
        int pageSize, 
        Guid? specializationId, 
        CancellationToken ct);
}
