using System.Security.Claims;
using Api.Helpers;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace Api.Utils;

public static class AuthPolicy
{
    public static void ConfigureAuthPolicy(IServiceCollection service)
    {
        var commonPolicy = new AuthorizationPolicyBuilder()
            .RequireClaim(ClaimTypes.Email)
            .RequireClaim(ClaimTypes.NameIdentifier)
            .Build();

        service.AddAuthorizationBuilder()
            .AddPolicy(AppConstants.BasePolicy, commonPolicy)

            .AddPolicy(AppConstants.PatientPolicy, policy =>
                policy
                    .RequireRole(nameof(Role.Patient))
                    .AddRequirements(commonPolicy.Requirements.ToArray()))

            .AddPolicy(AppConstants.DoctorPolicy, policy =>
                policy
                    .RequireRole(nameof(Role.Doctor))
                    .AddRequirements(commonPolicy.Requirements.ToArray()))

            .AddPolicy(AppConstants.ManagerPolicy, policy =>
                policy
                    .RequireRole(nameof(Role.Manager))
                    .AddRequirements(commonPolicy.Requirements.ToArray()))

            .AddPolicy(AppConstants.AdminPolicy, policy =>
                policy
                    .RequireRole(nameof(Role.Admin))
                    .AddRequirements(commonPolicy.Requirements.ToArray()))

            .AddPolicy(AppConstants.DoctorOrPatientPolicy, policy =>
                policy
                    .RequireRole(nameof(Role.Doctor), nameof(Role.Patient))
                    .AddRequirements(commonPolicy.Requirements.ToArray()))

            .AddPolicy(AppConstants.DoctorOrManagerPolicy, policy =>
                policy
                    .RequireRole(nameof(Role.Doctor), nameof(Role.Manager))
                    .AddRequirements(commonPolicy.Requirements.ToArray()));
    }
}