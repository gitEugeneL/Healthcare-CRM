using Domain.Abstractions.Errors;

namespace Domain.Doctors;

public static class DoctorErrors
{
    public static Error NotFound(Guid doctorId) =>
        Error.NotFound("Doctor.NotFound", $"The doctor with the identifierMa {doctorId} was not found");
}