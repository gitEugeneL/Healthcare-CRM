using Domain.Abstractions.Errors;

namespace Domain.Managers;

public static class ManagerErrors
{
    public static Error NotFound(Guid managerId) =>
        Error.NotFound("Manager.NotFound", $"The manager with the identifier {managerId} was not found");

    public static readonly Error InvalidRole = Error.Problem(
        "Manager.InvalidRole",
        "Manager role is invalid");
}