using Application.Common.Models;
using FluentValidation;
using MediatR;

namespace Application.Operations.Addresses.Commands.UpdateAddress;

public sealed record UpdateAddressCommand(
    string? Province,
    string? PostalCode,
    string? City,
    string? Street,
    string? Hose,
    string? Apartment
) : CurrentUser, IRequest<AddressResponse>;

public sealed class UpdateAddressValidator : AbstractValidator<UpdateAddressCommand>
{
    public UpdateAddressValidator()
    {
        RuleFor(a => a.Province)
            .MaximumLength(100);

        RuleFor(a => a.PostalCode)
            .MaximumLength(6)
            .Matches(@"^\d{2}-\d{3}$")
            .WithMessage("Valid postal code format: 00-000");

        RuleFor(a => a.City)
            .MaximumLength(100);

        RuleFor(a => a.Street)
            .MaximumLength(100);

        RuleFor(a => a.Hose)
            .MaximumLength(10);

        RuleFor(a => a.Apartment)
            .MaximumLength(10);
    }
}
