namespace Domain.Doctors;

public interface IDoctorRepository
{
    Task InsertDoctorAsync(Doctor doctor, CancellationToken ct);
    
    Task<bool> DoctorExistsAsync(Guid doctorId, CancellationToken ct);
    
    Task<Doctor?> FindDoctorByIdAsync(Guid doctorId, CancellationToken ct);

    Task<Doctor?> FindDoctorByIdWithTrackingAsync(Guid doctorId, CancellationToken ct);
    
    Task<Doctor?> FindDoctorForChangeStatus(Guid doctorId, CancellationToken ct);
    
    Task<(IReadOnlyList<Doctor> List, int Count)> GetDoctorsWithPaginationAsync(
        int pageNumber, 
        int pageSize, 
        Guid? specializationId,
        DoctorStatus? status,
        CancellationToken ct);
}
