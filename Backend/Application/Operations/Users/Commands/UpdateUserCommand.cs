using Application.Common.Models;

namespace Application.Operations.Users.Commands;

public abstract record UpdateUserCommand : CurrentUser
{
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? Phone { get; init; }
}
