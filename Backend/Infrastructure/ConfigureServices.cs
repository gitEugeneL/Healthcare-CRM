using Application.Common.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, 
        IConfiguration configuration)
    {
        services
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<IPasswordManager, PasswordManager>()
            .AddScoped<ITokenManager, TokenManager>();
        
        var connection = configuration.GetConnectionString("SQLServer")!;
        services.AddDbContext<DataContext>(option => 
            option.UseSqlServer(connection));
        
        return services;
    }
}
