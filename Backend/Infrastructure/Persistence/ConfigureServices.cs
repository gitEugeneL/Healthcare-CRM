using Domain.Abstractions;
using Domain.Appointments;
using Domain.Doctors;
using Domain.Managers;
using Domain.MedicalRecords;
using Domain.Offices;
using Domain.Patients;
using Domain.Specializations;
using Domain.Users;
using Domain.WorkSchedules;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Appointments;
using Persistence.Database;
using Persistence.Doctors;
using Persistence.Managers;
using Persistence.MedicalRecords;
using Persistence.Offices;
using Persistence.Patients;
using Persistence.Specializations;
using Persistence.Users;
using Persistence.WorkSchedules;

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
            .AddScoped<IMedicalRecordRepository, MedicalRecordRepository>()
            .AddScoped<ISpecializationRepository, SpecializationRepository>()
            .AddScoped<IAppointmentRepository, AppointmentRepository>()
            .AddScoped<IWorkScheduleRepository, WorkScheduleRepository>()
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<IDoctorRepository, DoctorRepository>()
            .AddScoped<IManagerRepository, ManagerRepository>()
            .AddScoped<IPatientRepository, PatientRepository>();
            
        return services;
    }
}
