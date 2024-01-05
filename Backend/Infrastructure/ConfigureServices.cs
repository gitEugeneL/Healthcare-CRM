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
            .AddScoped<IOfficeRepository, OfficeRepository>()
            .AddScoped<IMedicalRecordRepository, MedicalRecordRepository>()
            .AddScoped<ISpecializationRepository, SpecializationRepository>()
            .AddScoped<IAppointmentRepository, AppointmentRepository>()
            .AddScoped<IAppointmentSettingsRepository, AppointmentSettingsRepository>()
            .AddScoped<IAddressRepository, AddressRepository>()
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<IDoctorRepository, DoctorRepository>()
            .AddScoped<IManagerRepository, ManagerRepository>()
            .AddScoped<IPatientRepository, PatientRepository>()
            .AddScoped<IPasswordManager, PasswordManager>()
            .AddScoped<ITokenManager, TokenManager>();
        
        var connection = configuration.GetConnectionString("SQLServer")!;
        services.AddDbContext<DataContext>(option => 
            option.UseSqlServer(connection));
        
        /*** Init develop db data ***/
        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            ApplicationDbContextInitializer
                .Init(services.BuildServiceProvider().GetRequiredService<DataContext>());
        
        return services;
    }
}
