using Domain.Abstractions.Errors;
using Domain.Appointments;
using Domain.Common;

namespace Domain.MedicalRecords;

public sealed class MedicalRecord : BaseAuditableEntity
{
    private MedicalRecord() { }

    public string Title { get; private set; } = null!;
    public string DoctorNote { get; private set; } = null!;
    public string? RecommendationForPatient { get; private set; }
    public string? Diagnosis { get; private set; }
    public string? IcdCode { get; private set; }
    public DateOnly? FollowUpDate { get; private set; }    
    
    /*** Relations ***/
    public Appointment Appointment { get; private init; } = null!;
    public Guid AppointmentId { get; private init; }

    public static Result<MedicalRecord> Create(
        Guid appointmentId,
        string title, 
        string doctorNote, 
        string? recommendationForPatient,
        string? diagnosis, 
        string? icdCode, 
        DateOnly? followUpDate)
    {
        var medicalRecord = new MedicalRecord
        {
            Title = title.Trim().ToUpperInvariant(),
            DoctorNote = doctorNote.Trim(),
            RecommendationForPatient = recommendationForPatient?.Trim(),
            Diagnosis = diagnosis?.Trim(),
            IcdCode = icdCode?.Trim().ToUpperInvariant(),
            FollowUpDate = followUpDate,
            AppointmentId = appointmentId
        };

        if (followUpDate <= DateOnly.FromDateTime(DateTime.Now))
        {
            return Result.Failure<MedicalRecord>(MedicalRecordErrors.InvalidFollowUpDate);
        }
        
        return medicalRecord;
    }
    
    public void ChangeTittle(string title)
    {
        var normalized = title.Trim().ToUpperInvariant();
        
        if (Title == normalized)
            return;
        
        Title = normalized;
    }
    
    public void ChangeRecommendationForPatient(string recommendationForPatient)
    {
        var normalized = recommendationForPatient.Trim();
        
        if (RecommendationForPatient == normalized)
            return;
        
        RecommendationForPatient = normalized;   
    }
    
    public Result ChangeFollowUpDate(DateOnly followUpDate)
    {
        if (followUpDate <= DateOnly.FromDateTime(DateTime.Now))
            return Result.Failure(MedicalRecordErrors.InvalidFollowUpDate);
        
        FollowUpDate = followUpDate;
        return Result.Success();
    }
}
