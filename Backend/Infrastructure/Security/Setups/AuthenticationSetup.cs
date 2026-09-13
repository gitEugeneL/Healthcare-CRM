using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Security.Setups;

internal static class AuthenticationSetup
{
    internal static void AddSecurityAuthentication(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(3),

                    ValidIssuer = configuration["Authentication:Issuer"] ??
                                  throw new ApplicationException("Issuer not found in configuration"),

                    ValidAudience = configuration["Authentication:Audience"] ??
                                    throw new ApplicationException("Audience not found in configuration"),

                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                        .GetBytes(configuration["Authentication:AccessToken:SecurityKey"] ??
                                  throw new ApplicationException("SecurityKey not found in configuration")))
                };
            });

    }
}