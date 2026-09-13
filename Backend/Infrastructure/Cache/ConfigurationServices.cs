using Application.Abstractions.Cache;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Cache;

public static class ConfigurationServices
{
    public static IServiceCollection AddCacheServices(this IServiceCollection services, IConfiguration config)
    {
        var redisConnectionString = config.GetConnectionString("Redis") 
            ?? throw new InvalidOperationException("Redis connection string not found");
        
        services.AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect(redisConnectionString));
        
        services.AddSingleton<ICacheService, CacheService>();
        
        return services;
    }
}