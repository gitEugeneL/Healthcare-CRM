namespace Application.Operations.Users;

public abstract record UserResponse
{
    public Guid UserId { get; protected set; }
    public string Email { get; set; } = string.Empty;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Phone { get; set; }
}
