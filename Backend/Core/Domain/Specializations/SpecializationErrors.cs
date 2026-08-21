using Domain.Abstractions.Errors;

namespace Domain.Specializations;

public static class SpecializationErrors
{
    public static Error NotFound(Guid specializationId) => Error.NotFound(
        "Specialization.NotFound", 
        $"The specialization with the identifier {specializationId} was not found");
    
    public static Error AlreadyExist(string name) => Error.Conflict(
            "Specialization.AlreadyExist", 
            $"The Specialization with the name {name} already exists");
    
    public static Error NotEmpty(Guid specializationId) => Error.Problem(
            "Specialization.NotEmpty", 
            $"The Specialization with the identifier {specializationId} is not empty");
    
    public static Error DoctorAlreadyIncluded(Guid doctorId) => Error.Conflict(
            "Specialization.DoctorAlreadyIncluded", 
            $"The Doctor with the identifier {doctorId} is already included in this specialization");
    
    public static Error DoctorNotFound(Guid doctorId) => Error.NotFound(
            "Specialization.DoctorNotFound", 
            $"The Doctor with the identifier {doctorId} was not found");
}
