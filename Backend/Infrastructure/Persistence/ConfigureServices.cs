using Application.Common.Interfaces;
using Domain.Abstractions;
using Domain.Doctors;
using Domain.Managers;
using Domain.Offices;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Managers;
using Persistence.Offices;
using Persistence.Persistence;
using Persistence.Security;
using Persistence.Users;

namespace Persistence;

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
            .AddScoped<IOfficeRepository, OfficeRepository>()
            // .AddScoped<IMedicalRecordRepository, MedicalRecordRepository>()
            .AddScoped<ISpecializationRepository, SpecializationRepository>()
            // .AddScoped<IAppointmentRepository, AppointmentRepository>()
            // .AddScoped<IAppointmentSettingsRepository, AppointmentSettingsRepository>()
            // .AddScoped<IAddressRepository, AddressRepository>()
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<IDoctorRepository, DoctorRepository>()
            .AddScoped<IManagerRepository, ManagerRepository>();
            // .AddScoped<IPatientRepository, PatientRepository>();
            
        /*** Security ***/
        services
            .AddSingleton<IPasswordManager, PasswordManager>()
            .AddSingleton<ITokenManager, TokenManager>();
            
        return services;
    }
}
