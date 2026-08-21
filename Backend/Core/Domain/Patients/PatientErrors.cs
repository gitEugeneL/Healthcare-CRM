using Domain.Abstractions.Errors;

namespace Domain.Patients;

public static class PatientErrors
{
    public static Error NotFound(Guid patientId) => Error.NotFound(
        "Patient.NotFound", 
        $"The patient with the identifierMa {patientId} was not found");
    
    public static Error AlreadyDeactivated(Guid patientId) => Error.Conflict(
        "Patient.AlreadyDeactivated", 
        $"The patient with the identifier {patientId} already deactivated");
}