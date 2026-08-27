using Domain.Abstractions.Errors;

namespace Domain.Doctors;

public static class DoctorErrors
{
    public static Error NotFound(Guid doctorId) => Error.NotFound(
        "Doctor.NotFound", 
        $"The doctor with the identifierMa {doctorId} was not found or not active");
    
    public static readonly Error EmptyWorSchedule = Error.Problem(
        "Doctor.WorkScheduleEmpty",
        "Doctor have not got work schedule yet");
    
    public static readonly Error EmptySpecializationList = Error.Problem(
        "Doctor.SpecializationsListEmpty",
        "Doctor have not got any specializations yet");
}
