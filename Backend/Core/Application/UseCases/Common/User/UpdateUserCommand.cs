namespace Application.UseCases.Common.User;

public abstract record UpdateUserCommand(
    string? FirstName,
    string? LastName,
    string? Phone
);