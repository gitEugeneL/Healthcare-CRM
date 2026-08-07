using Domain.Abstractions.Errors;

namespace Domain.Offices;

public static class OfficeErrors
{
    public static Error NotFound(Guid officeId) =>
        Error.NotFound("Office.NotFound", $"The office with the identifier {officeId} was not found");
    
    public static Error AlreadyExist(int number) =>
        Error.Conflict("Office.AlreadyExist", $"The office with the number {number} already exists");
}
