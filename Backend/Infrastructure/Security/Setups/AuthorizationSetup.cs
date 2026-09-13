using System.Security.Claims;
using Domain.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Security.Setups;

internal static class AuthorizationSetup
{
    internal static void AddSecurityAuthorization(this IServiceCollection services)
    {
        var commonRequirements = new IAuthorizationRequirement[]
        {
            new ClaimsAuthorizationRequirement(ClaimTypes.Email, allowedValues: null),
            new ClaimsAuthorizationRequirement(ClaimTypes.NameIdentifier, allowedValues: null)
        };

        services.AddAuthorizationBuilder()
            .AddPolicy(AuthTags.BasePolicy, policy => policy.AddRequirements(commonRequirements))

            .AddPolicy(AuthTags.PatientPolicy, policy =>
                policy.RequireRole(nameof(UserAuthRole.Patient))
                      .AddRequirements(commonRequirements))

            .AddPolicy(AuthTags.DoctorPolicy, policy =>
                policy.RequireRole(nameof(UserAuthRole.Doctor))
                      .AddRequirements(commonRequirements))

            .AddPolicy(AuthTags.ManagerPolicy, policy =>
                policy.RequireRole(nameof(UserAuthRole.Manager))
                      .AddRequirements(commonRequirements))

            .AddPolicy(AuthTags.AdminPolicy, policy =>
                policy.RequireRole(nameof(UserAuthRole.Admin))
                      .AddRequirements(commonRequirements))

            .AddPolicy(AuthTags.ManagerOrPatientPolicy, policy =>
                policy.RequireRole(nameof(UserAuthRole.Manager), nameof(UserAuthRole.Patient))
                      .AddRequirements(commonRequirements))

            .AddPolicy(AuthTags.DoctorOrPatientPolicy, policy =>
                policy.RequireRole(nameof(UserAuthRole.Doctor), nameof(UserAuthRole.Patient))
                      .AddRequirements(commonRequirements))

            .AddPolicy(AuthTags.DoctorOrManagerPolicy, policy =>
                policy.RequireRole(nameof(UserAuthRole.Doctor), nameof(UserAuthRole.Manager))
                      .AddRequirements(commonRequirements));
    }
}