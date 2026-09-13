using Persistence.Database;
using Scalar.AspNetCore;

namespace Api.Setups;

internal static class DevSetup
{
    internal static WebApplication UeDevConfiguration(this WebApplication app)
    {
        if (!app.Environment.IsDevelopment()) 
            return app;
        
        DevInitializer.InitializeDatabase(app.Services);

        app.MapOpenApi();

        app.MapScalarApiReference("/scalar");

        return app;
    }
}