using Domain.Offices;

namespace Application.UseCases.Offices;

public sealed record OfficeResponse(Guid OfficeId, string Name, int Number, bool IsAvailable)
{
    public static OfficeResponse FromOffice(Office office)
    {
        return new OfficeResponse(office.Id, office.Name, office.Number, office.IsAvailable);
    }
}