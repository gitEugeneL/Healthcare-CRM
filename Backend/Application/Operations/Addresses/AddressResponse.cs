using Domain.Entities;

namespace Application.Operations.Addresses;

public sealed record AddressResponse
{
    public string? Province { get; set; }
    public string? PostalCode { get; set; }
    public string? City { get; set; }
    public string? Street { get; set; }
    public string? Hose { get; set; }
    public string? Apartment { get; set; }

    public AddressResponse ToAddressResponse(Address address)
    {
        Province = address.Province;
        PostalCode = address.PostalCode;
        City = address.City;
        Street = address.Street;
        Hose = address.Hose;
        Apartment = address.Apartment;

        return this;
    }
}
