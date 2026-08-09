using Domain.Abstractions.Errors;

namespace Domain.Doctors;

public static class DoctorErrors
{
    public static Error NotFound(Guid doctorId) => Error.NotFound(
        "Doctor.NotFound", 
        $"The doctor with the identifierMa {doctorId} was not found");
    
    public static readonly Error EmptyWorSchedule = Error.Problem(
        "Doctor.WorkScheduleEmpty",
        "Doctor have not got work schedule yet");
    
    public static readonly Error EmptySpecializationList = Error.Problem(
        "Doctor.SpecializationsListEmpty",
        "Doctor have not got any specializations yet");
    
    public static readonly Error SpecializationAlreadyExists = Error.Conflict(
        "Doctor.SpecializationAlreadyExists",
        "Doctor already has this specialization");

    public static readonly Error SpecializationNotFound = Error.NotFound(
        "Doctor.SpecializationNotFound",
        "Specialization not found for this doctor");
}
