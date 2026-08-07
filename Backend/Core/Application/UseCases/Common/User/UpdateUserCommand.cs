namespace Application.UseCases.Common.User;

public abstract record UpdateUserCommand(
    Guid UserId,
    string? FirstName,
    string? LastName,
    string? Phone
);