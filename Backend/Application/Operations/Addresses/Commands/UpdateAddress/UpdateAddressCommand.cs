using System.ComponentModel.DataAnnotations;
using Application.Common.Models;
using MediatR;

namespace Application.Operations.Addresses.Commands.UpdateAddress;

public sealed record UpdateAddressCommand : CurrentUser, IRequest<AddressResponse>
{
    [MaxLength(100)]
    public string? Province { get; init; }

    [MaxLength(6)]
    [RegularExpression(
        @"^\d{2}-\d{3}$",
        ErrorMessage = "Valid postal code format: 00-000"
    )]
    public string? PostalCode { get; init; }
    
    [MaxLength(100)]
    public string? City { get; init; }
   
    [MaxLength(100)]
    public string? Street { get; init; }
    
    [MaxLength(10)]
    public string? Hose { get; init; }

    [MaxLength(10)]
    public string? Apartment { get; init; }
}
