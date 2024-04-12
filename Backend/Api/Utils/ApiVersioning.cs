using Asp.Versioning;
using Asp.Versioning.Builder;

namespace Api.Utils;

public static class ApiVersioning
{
    public static ApiVersionSet VersionSet(IEndpointRouteBuilder app)
    {
        return app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1))
            .ReportApiVersions()
            .Build();    
    }
}