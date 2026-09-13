using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;

namespace Api.Setups;

internal static class RateLimiterSetup
{
    internal const string FixedRateLimiter = "fixed";
    
    public static IServiceCollection AddRateLimiterSetup(this IServiceCollection services)
    {
        services.AddRateLimiter(rateLimitOptions =>
        {
            rateLimitOptions.AddFixedWindowLimiter(FixedRateLimiter, options =>
            {
                options.PermitLimit = 10;
                options.Window = TimeSpan.FromSeconds(10);
                options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                options.QueueLimit = 5;
            });
        });

        return services;
    }
}