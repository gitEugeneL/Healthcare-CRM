using Api.Setups;

namespace Api;

public static class ConfigureServices
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOpenApi();
        
        services.AddRateLimiterSetup();
        
        services.AddExceptionHandler<GlobalExceptionHandlerSetup>();
        services.AddProblemDetails();
        
        return services;
    }
}
