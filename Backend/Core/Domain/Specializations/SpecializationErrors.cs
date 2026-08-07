using Domain.Abstractions.Errors;

namespace Domain.Specializations;

public static class SpecializationErrors
{
    public static Error NotFound(Guid specializationId) => Error.NotFound(
        "Specialization.NotFound", 
        $"The specialization with the identifier {specializationId} was not found");
    
    public static Error AlreadyExist(int name) => Error.Conflict(
            "Specialization.AlreadyExist", 
            $"The Specialization with the name {name} already exists");
}
