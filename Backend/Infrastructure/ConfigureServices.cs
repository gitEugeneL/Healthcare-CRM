using Domain.Abstractions;
using Domain.Offices;
using Infrastructure.Offices;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        /*** Database ***/
        services.AddDbContext<DataContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("PSQL")));
        
        /*** Unit of Work ***/
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<DataContext>());
        
        /*** Repositories ***/
        services
            .AddScoped<IOfficeRepository, OfficeRepository>();
            // .AddScoped<IMedicalRecordRepository, MedicalRecordRepository>()
            // .AddScoped<ISpecializationRepository, SpecializationRepository>()
            // .AddScoped<IAppointmentRepository, AppointmentRepository>()
            // .AddScoped<IAppointmentSettingsRepository, AppointmentSettingsRepository>()
            // .AddScoped<IAddressRepository, AddressRepository>()
            // .AddScoped<IUserRepository, UserRepository>()
            // .AddScoped<IDoctorRepository, DoctorRepository>()
            // .AddScoped<IManagerRepository, ManagerRepository>()
            // .AddScoped<IPatientRepository, PatientRepository>();
            
        /*** Security ***/
        // services
            // .AddSingleton<IPasswordManager, PasswordManager>();
            // .AddSingleton<ITokenManager, TokenManager>();
            
        return services;
    }
}
