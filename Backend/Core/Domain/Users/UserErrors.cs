using Domain.Abstractions.Errors;

namespace Domain.Users;

public static class UserErrors
{
    public static Error AlreadyExist(string email) =>
        Error.Conflict("User.AlreadyExist", $"The user with email {email} already exists");
}
