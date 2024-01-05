using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Application.Operations.Auth.Commands.Register;

public record RegisterCommand : IRequest<Guid>
{
    [Required]
    [EmailAddress]
    public required string Email { get; init; }
    
    [Required]
    [MinLength(8)]
    [MaxLength(20)]
    [RegularExpression(
        @"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d!$%@#£€*?&]+$", 
        ErrorMessage = "The password must contain at least one letter, one special character, and one digit."
    )]
    public required string Password { get; init; }
}
