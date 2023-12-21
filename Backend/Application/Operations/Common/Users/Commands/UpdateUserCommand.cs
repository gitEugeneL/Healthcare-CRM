using System.ComponentModel.DataAnnotations;

namespace Application.Operations.Common.Users.Commands;

public abstract record UpdateUserCommand
{
    public Guid CurrentUserId { get; private set; }
    
    [MaxLength(50)]
    public string? FirstName { get; init; }
    
    [MaxLength(100)]
    public string? LastName { get; init; }
    
    [MaxLength(12)]
    [RegularExpression(
        "^[+]?\\d+$",
        ErrorMessage = "Phone number should start with + (optional) and contain only digits."
    )]
    public string? Phone { get; init; }
    
    public void SetCurrentUserId(string id) => CurrentUserId = Guid.Parse(id);
}
