namespace Application.UseCases.Common.User;

public abstract record UserResponse(
    Guid UserId, 
    string Email, 
    string? FirstName, 
    string? LastName, 
    string? Phone
);