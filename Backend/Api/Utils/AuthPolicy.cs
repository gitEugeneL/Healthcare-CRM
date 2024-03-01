using System.Security.Claims;
using Domain.Enums;

namespace Api.Utils;

public static class AuthPolicy
{
    public const string PatientPolicy = "patient-policy";
    public const string DoctorPolicy = "doctor-policy";
    public const string ManagerPolicy = "manager-policy";
    public const string AdminPolicy = "admin-policy";
    public const string DoctorOrPatientPolicy = "doctor-or-patient-policy";
    
    public static void ConfigureAuthPolicy(IServiceCollection service)
    {
        service.AddAuthorizationBuilder()
            .AddPolicy(PatientPolicy, policy =>
                policy
                    .RequireRole(Role.Patient.ToString())
                    .RequireClaim(ClaimTypes.Email)
                    .RequireClaim(ClaimTypes.NameIdentifier))
            .AddPolicy(DoctorPolicy, policy =>
                policy
                    .RequireRole(Role.Doctor.ToString())
                    .RequireClaim(ClaimTypes.Email)
                    .RequireClaim(ClaimTypes.NameIdentifier))
            .AddPolicy(ManagerPolicy, policy =>
                policy
                    .RequireRole(Role.Manager.ToString())
                    .RequireClaim(ClaimTypes.Email)
                    .RequireClaim(ClaimTypes.NameIdentifier))
            .AddPolicy(AdminPolicy, policy =>
                policy
                    .RequireRole(Role.Admin.ToString())
                    .RequireClaim(ClaimTypes.Email)
                    .RequireClaim(ClaimTypes.NameIdentifier))
            .AddPolicy(DoctorOrPatientPolicy, policy =>
                policy
                    .RequireRole(Role.Doctor.ToString(), Role.Patient.ToString())
                    .RequireClaim(ClaimTypes.Email)
                    .RequireClaim(ClaimTypes.NameIdentifier));
    }
}