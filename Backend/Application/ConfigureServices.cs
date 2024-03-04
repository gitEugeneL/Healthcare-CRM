using System.Reflection;
using Application.Common.Interfaces;
using Application.Common.Services;
using Application.Operations.Users.Commands;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IRefreshTokenService, RefreshTokenService>();

        /*** FluentValidation files register ***/
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        
        
        /*** Mediatr config ***/
        services.AddMediatR(cnf => 
            cnf.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        
        return services;
    }
}
