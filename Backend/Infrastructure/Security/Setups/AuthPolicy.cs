using System.Security.Claims;
using Domain.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Security.Utils;

namespace Security.Setups;

public static class AuthPolicy
{
    public static void ConfigureAuthPolicy(IServiceCollection service)
    {
        var commonPolicy = new AuthorizationPolicyBuilder()
            .RequireClaim(ClaimTypes.Email)
            .RequireClaim(ClaimTypes.NameIdentifier)
            .Build();

        service.AddAuthorizationBuilder()
            .AddPolicy(AuthTags.BasePolicy, commonPolicy)

            .AddPolicy(AuthTags.PatientPolicy, policy =>
                policy
                    .RequireRole(nameof(UserAuthRole.Patient))
                    .AddRequirements(commonPolicy.Requirements.ToArray()))

            .AddPolicy(AuthTags.DoctorPolicy, policy =>
                policy
                    .RequireRole(nameof(UserAuthRole.Doctor))
                    .AddRequirements(commonPolicy.Requirements.ToArray()))

            .AddPolicy(AuthTags.ManagerPolicy, policy =>
                policy
                    .RequireRole(nameof(UserAuthRole.Manager))
                    .AddRequirements(commonPolicy.Requirements.ToArray()))

            .AddPolicy(AuthTags.AdminPolicy, policy =>
                policy
                    .RequireRole(nameof(UserAuthRole.Admin))
                    .AddRequirements(commonPolicy.Requirements.ToArray()))

            .AddPolicy(AuthTags.ManagerOrPatientPolicy, policy =>
                policy
                    .RequireRole(nameof(UserAuthRole.Manager), nameof(UserAuthRole.Patient))
                    .AddRequirements(commonPolicy.Requirements.ToArray()))
            
            .AddPolicy(AuthTags.DoctorOrPatientPolicy, policy =>
                policy
                    .RequireRole(nameof(UserAuthRole.Doctor), nameof(UserAuthRole.Patient))
                    .AddRequirements(commonPolicy.Requirements.ToArray()))

            .AddPolicy(AuthTags.DoctorOrManagerPolicy, policy =>
                policy
                    .RequireRole(nameof(UserAuthRole.Doctor), nameof(UserAuthRole.Manager))
                    .AddRequirements(commonPolicy.Requirements.ToArray()));
        
    }
}