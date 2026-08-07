using Application.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Security;

public static class ConfigureServices
{
    public static IServiceCollection AddSecurityServices(this IServiceCollection services)
    {
        services
            .AddSingleton<IPasswordManager, PasswordManager>()
            .AddSingleton<ITokenManager, TokenManager>();
            
        return services;
    }
}
