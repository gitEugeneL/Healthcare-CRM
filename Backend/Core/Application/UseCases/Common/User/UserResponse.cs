namespace Application.UseCases.Common.User;

public abstract record UserResponse(
    string Email, 
    string? FirstName, 
    string? LastName, 
    string? Phone
);