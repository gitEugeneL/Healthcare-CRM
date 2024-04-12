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
                    .RequireRole(Role.Patient.ToString())
                    .AddRequirements(commonPolicy.Requirements.ToArray()))

            .AddPolicy(AppConstants.DoctorPolicy, policy =>
                policy
                    .RequireRole(Role.Doctor.ToString())
                    .AddRequirements(commonPolicy.Requirements.ToArray()))

            .AddPolicy(AppConstants.ManagerPolicy, policy =>
                policy
                    .RequireRole(Role.Manager.ToString())
                    .AddRequirements(commonPolicy.Requirements.ToArray()))

            .AddPolicy(AppConstants.AdminPolicy, policy =>
                policy
                    .RequireRole(Role.Admin.ToString())
                    .AddRequirements(commonPolicy.Requirements.ToArray()))

            .AddPolicy(AppConstants.DoctorOrPatientPolicy, policy =>
                policy
                    .RequireRole(Role.Doctor.ToString(), Role.Patient.ToString())
                    .AddRequirements(commonPolicy.Requirements.ToArray()))

            .AddPolicy(AppConstants.DoctorOrManagerPolicy, policy =>
                policy
                    .RequireRole(Role.Doctor.ToString(), Role.Manager.ToString())
                    .AddRequirements(commonPolicy.Requirements.ToArray()));
    }
}