using Domain.Common;

namespace Domain.Addresses;

public sealed class Address : BaseAuditableEntity
{
    private Address() { }

    public string Province { get; private set; } = null!;
    public string PostalCode { get; private set; } = null!;
    public string City { get; private set; } = null!;
    public string Street { get; private set; } = null!;
    public string Hose { get; private set; } = null!;
    public string? Apartment { get; private set; }
    
    public static Address Create(
        string province,
        string postalCode,
        string city,
        string street,
        string hose,
        string? apartment)
    {
        var address = new Address
        {
            Province = province.Trim(),
            PostalCode = postalCode.Trim(),
            City = city.Trim(),
            Street = street.Trim(),
            Hose = hose.Trim(),
            Apartment = apartment?.Trim()
        };
        return address;
    }
    
    public void ChangeProvince(string province)
    {
        var normalized = province.Trim();
        
        if (Province == normalized)
            return;

        Province = normalized;
    }

    public void ChangePostalCode(string postalCode)
    {
        var normalized = postalCode.Trim();
        
        if (PostalCode == normalized)
            return;
        
        PostalCode = normalized;
    }
    
    public void ChangeCity(string city)
    {
        var normalized = city.Trim();
        
        if (City == normalized)
            return;
        
        City = normalized;
    }

    public void ChangeHose(string hose)
    {
        var normalized = hose.Trim();
        
        if (Hose == normalized)
            return;

        Hose = normalized;
    }
    
    public void ChangeStreet(string street)
    {
        var normalized = street.Trim();
        
        if (Street == normalized)
            return;
        
        Street = normalized;
    }
    
    public void ChangeApartment(string apartment)
    {
        var normalized = apartment?.Trim();
        
        if (Apartment == normalized)
            return;

        Apartment = normalized;
    }
}
