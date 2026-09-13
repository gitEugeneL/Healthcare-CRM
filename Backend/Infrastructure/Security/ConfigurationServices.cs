using Application.Abstractions.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Security.Services;
using Security.Setups;

namespace Security;

public static class ConfigurationServices
{
    public static IServiceCollection AddSecurityServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSecurityAuthentication(configuration);
        services.AddSecurityAuthorization();
        
        services.AddSingleton<AttemptLimiter>();
        
        services.AddScoped<ILockoutService, LockoutService>();
        services.AddScoped<IConfirmationService, ConfirmationService>();
        services.AddScoped<IPasswordService, PasswordService>();
        services.AddScoped<ITokenService, TokenService>();
        
        return services;
    }
}