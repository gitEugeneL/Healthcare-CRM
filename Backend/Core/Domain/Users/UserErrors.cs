using Domain.Abstractions.Errors;

namespace Domain.Users;

public static class UserErrors
{
    public static readonly Error NotFoundOrCodeIsInvalid = Error.NotFound(
        "User.NotFound", 
        $"User was not found or email isn't confirmed or code is invalid");
    
    public static readonly Error NotFoundOrInvalidOrExpiredToken = Error.NotFound(
        "User.NotFound", 
        $"User was not found or token is invalid or token has expired");
    
    public static Error AlreadyExist(string email) => Error.Conflict(
        "User.AlreadyExist", 
            $"The user with email {email} already exists");
    
    public static Error NotFoundOfInvalidPassword(string email) => Error.NotFound(
        "User.NotFoundOfInvalidPassword", 
        $"The user with email {email} was not found or the password is invalid");
}
