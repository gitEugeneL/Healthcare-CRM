// using System.Security.Claims;
// using Domain.Enums;
// using Microsoft.AspNetCore.Authorization;
//
// namespace Api.Utils;
//
// internal static class AuthPolicy
// {
//     internal static void ConfigureAuthPolicy(IServiceCollection service)
//     {
//         var commonPolicy = new AuthorizationPolicyBuilder()
//             .RequireClaim(ClaimTypes.Email)
//             .RequireClaim(ClaimTypes.NameIdentifier)
//             .Build();
//
//         service.AddAuthorizationBuilder()
//             .AddPolicy(AuthTags.BasePolicy, commonPolicy)
//
//             .AddPolicy(AuthTags.PatientPolicy, policy =>
//                 policy
//                     .RequireRole(nameof(Role.Patient))
//                     .AddRequirements(commonPolicy.Requirements.ToArray()))
//
//             .AddPolicy(AuthTags.DoctorPolicy, policy =>
//                 policy
//                     .RequireRole(nameof(Role.Doctor))
//                     .AddRequirements(commonPolicy.Requirements.ToArray()))
//
//             .AddPolicy(AuthTags.ManagerPolicy, policy =>
//                 policy
//                     .RequireRole(nameof(Role.Manager))
//                     .AddRequirements(commonPolicy.Requirements.ToArray()))
//
//             .AddPolicy(AuthTags.AdminPolicy, policy =>
//                 policy
//                     .RequireRole(nameof(Role.Admin))
//                     .AddRequirements(commonPolicy.Requirements.ToArray()))
//
//             .AddPolicy(AuthTags.DoctorOrPatientPolicy, policy =>
//                 policy
//                     .RequireRole(nameof(Role.Doctor), nameof(Role.Patient))
//                     .AddRequirements(commonPolicy.Requirements.ToArray()))
//
//             .AddPolicy(AuthTags.DoctorOrManagerPolicy, policy =>
//                 policy
//                     .RequireRole(nameof(Role.Doctor), nameof(Role.Manager))
//                     .AddRequirements(commonPolicy.Requirements.ToArray()));
//     }
// }