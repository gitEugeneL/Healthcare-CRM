using System.ComponentModel.DataAnnotations;
using Application.Common.Models;

namespace Application.Operations.Users.Commands;

public abstract record UpdateUserCommand : CurrentUser
{
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
}
