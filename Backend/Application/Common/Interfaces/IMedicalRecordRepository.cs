using Domain.Entities;

namespace Application.Common.Interfaces;

public interface IMedicalRecordRepository
{
    Task<MedicalRecord> CreateMedicalRecordAsync(MedicalRecord medicalRecord, CancellationToken cancellationToken);

    Task<MedicalRecord> UpdateMedicalRecordAsync(MedicalRecord medicalRecord, CancellationToken cancellationToken);
    
    Task<MedicalRecord?> FindMedicalRecordByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<MedicalRecord?> FindMedicalRecordByAppointmentIdAsync(Guid id, CancellationToken cancellationToken);

    Task<(IEnumerable<MedicalRecord> List, int Count)> GetMedicalRecordsForPatientWithPaginationAsync(
        CancellationToken cancellationToken,
        int pageNumber,
        int pageSize,
        Guid patientId,
        bool sortByDate = false,
        bool sortOrderAsc = true
    );

    Task<(IEnumerable<MedicalRecord> List, int Count)> GetMedicalRecordsForDoctorWithPaginationAsync(
        CancellationToken cancellationToken,
        int pageNumber,
        int pageSize,
        Guid doctorId,
        Guid? patientId = null,
        bool sortByDate = false,
        bool sortOrderAsc = true
    );
}
