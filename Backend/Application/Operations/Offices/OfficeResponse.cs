using Domain.Entities;

namespace Application.Operations.Offices;

public sealed record OfficeResponse
{
    public Guid OfficeId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Number { get; set; }
    public bool IsAvailable { get; set; }

    public OfficeResponse ToOfficeResponse(Office office)
    {
        OfficeId = office.Id;
        Name = office.Name;
        Number = office.Number;
        IsAvailable = office.IsAvailable;

        return this;
    }
}
