using Application.Abstractions.Mail;
using Microsoft.Extensions.DependencyInjection;

namespace Mail;

public static class ConfigurationServices
{
    public static IServiceCollection AddMailServices(this IServiceCollection services)
    {
        services.AddScoped<IMailService, MailService>();
        
        return services;
    }
}