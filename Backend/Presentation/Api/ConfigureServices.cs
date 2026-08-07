namespace Api;

public static class ConfigureServices
{
    
    public static IServiceCollection AddApiServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddOpenApi();
        
        // services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            // .AddJwtBearer(options =>
            // {
                // options.TokenValidationParameters = new TokenValidationParameters
                // {
                    // ValidateIssuerSigningKey = true,
                    // IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                        // .GetBytes(configuration.GetSection("Authentication:Key").Value!)),
                    // ValidateIssuer = false,
                    // ValidateAudience = false,
                    // ValidateLifetime = true,
                    // ClockSkew = TimeSpan.FromMinutes(1) // allowed time deviation, 5min - default
                // };
            // });
        
        /*** Auth policies configure ***/
        // AuthPolicy.ConfigureAuthPolicy(services);
        
        return services;
    }
}
