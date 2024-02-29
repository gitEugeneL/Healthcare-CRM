using System.Security.Claims;
using Domain.Enums;

namespace Api.Utils;

public static class AuthPolicy
{
    public const string PatientPolicy = "patient-policy";

    public static void ConfigureAuthPolicy(IServiceCollection service)
    {
        service.AddAuthorizationBuilder()
            .AddPolicy(PatientPolicy, policy =>
                policy
                    .RequireRole(Role.Patient.ToString())
                    .RequireClaim(ClaimTypes.Email)
                    .RequireClaim(ClaimTypes.NameIdentifier));
    }
}