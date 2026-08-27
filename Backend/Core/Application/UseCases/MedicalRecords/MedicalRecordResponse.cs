using Domain.MedicalRecords;

namespace Application.UseCases.MedicalRecords;

public sealed record MedicalRecordResponse(
        Guid MedicalRecordId,
        Guid AppointmentId,
        Guid PatientId,
        Guid DoctorId,
        string Title,
        string DoctorNote,
        string? RecommendationForPatient,
        string? Diagnosis,
        string? IcdCode,
        DateOnly? FollowUpDate)
{
    public static MedicalRecordResponse FromMedicalRecord(MedicalRecord medicalRecord)
    {
        return new MedicalRecordResponse(
            MedicalRecordId: medicalRecord.Id,
            AppointmentId: medicalRecord.AppointmentId,
            PatientId: medicalRecord.Appointment.PatientId,
            DoctorId: medicalRecord.Appointment.DoctorId,
            Title: medicalRecord.Title,
            DoctorNote: medicalRecord.DoctorNote,
            RecommendationForPatient: medicalRecord.RecommendationForPatient,
            Diagnosis: medicalRecord.Diagnosis,
            IcdCode: medicalRecord.IcdCode,
            FollowUpDate: medicalRecord.FollowUpDate);
    }
} 
