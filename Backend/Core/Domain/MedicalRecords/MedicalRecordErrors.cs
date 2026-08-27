using Domain.Abstractions.Errors;

namespace Domain.MedicalRecords;

public static class MedicalRecordErrors
{
    public static Error NotFound(Guid medicalRecordId) => Error.NotFound(
        "MedicalRecord.NotFound", 
        $"The medical record with the identifier {medicalRecordId} was not found");
    
    public static Error AlreadyExist(Guid appointmentId) => Error.Conflict(
        "MedicalRecord.AlreadyExist", 
        $"The MedicalRecord with appointment identifier {appointmentId} already exists");
    
    public static readonly Error InvalidFollowUpDate = Error.Problem(
        "MedicalRecord.InvalidFollowUpDate",
        "Follow-up date must be in the future");
}