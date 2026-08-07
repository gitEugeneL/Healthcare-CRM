namespace Application.UseCases.Common.User;

public abstract record CreateUserCommand(
    string Email, 
    string Password,
    string? FirstName,
    string? LastName,
    string? Phone);

