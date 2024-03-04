namespace Application.Operations.Users.Commands;

public abstract record CreateUserCommand
{
    public required string Email { get; init; }
    public required string Password { get; init; }
}
