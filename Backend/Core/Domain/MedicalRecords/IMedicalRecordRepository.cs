namespace Domain.MedicalRecords;

public interface IMedicalRecordRepository
{
    Task InsertMedicalRecordAsync(MedicalRecord medicalRecord, CancellationToken ct);
    
    Task<bool> IsMedicalRecordByAppointmentIdExistsAsync(Guid appointmentId, CancellationToken ct);
    
    Task<MedicalRecord?> FindMedicalRecordByIdWithTrackingAsync(Guid medicalRecordId, CancellationToken ct);
    
    Task<(IReadOnlyList<MedicalRecord> List, int Count)> GetAllMedicalRecordsWithPaginationAsync(int pageNumber,
        int pageSize,
        Guid? doctorId,
        Guid? patientId,
        CancellationToken ct);
}
