using Application.Common.Interfaces;
using Domain.Entities;
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
        
        return services;
    }
}
