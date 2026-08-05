using Api.Endpoints.Offices;

namespace Api.Endpoints;

public static class OfficeEndpoints
{
    public static void MapEndpoints(IEndpointRouteBuilder app)
    {
        ChangeOfficeStatus.MapEndpoint(app);
        CreateOffice.MapEndpoint(app);
        GetAllOffices.MapEndpoint(app);
        UpdateOfficeName.MapEndpoint(app);
    }
}