using Domain.Entities;

namespace Application.Operations.MedicalRecords;

public sealed record MedicalRecordResponse
{
    public Guid MedicalRecordId { get; set; }
    public Guid UserPatientId { get; set; }
    public Guid UserDoctorId { get; set; }
    public Guid AppointmentId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string DoctorNote { get; set; } = string.Empty;
    public DateTime Created { get; set; }

    public MedicalRecordResponse ToMedicalRecordResponse(MedicalRecord medicalRecord)
    {
        MedicalRecordId = medicalRecord.Id;
        UserPatientId = medicalRecord.UserPatient.UserId;
        UserDoctorId = medicalRecord.UserDoctor.UserId;
        AppointmentId = medicalRecord.AppointmentId;
        Title = medicalRecord.Title;
        DoctorNote = medicalRecord.DoctorNote;
        Created = medicalRecord.Created;
        
        return this;
    }
}
