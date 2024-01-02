using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class AppointmentRepository(DataContext dataContext) : IAppointmentRepository
{
    public async Task<List<DoctorHours>> FindFreeHoursAsync(
        UserDoctor doctor, DateOnly date, CancellationToken cancellationToken)
    {
        var busyTime = await dataContext.Appointments
            .Where(a => 
                a.UserDoctorId == doctor.Id 
                && a.Date == date 
                && a.IsCanceled == false
            )
            .Select(a => a.StartTime)
            .ToListAsync(cancellationToken);
        
        var startTime = doctor.AppointmentSettings.StartTime;
        var endTime = doctor.AppointmentSettings.EndTime;
        var interval = (double) doctor.AppointmentSettings.Interval;
        
        var freeTime = new List<DoctorHours>();
        while (startTime < endTime)
        {
            if (!busyTime.Contains(startTime))
                freeTime.Add(new DoctorHours(startTime, startTime.AddMinutes(interval)));

            startTime = startTime.AddMinutes(interval);
        }
        return freeTime;
    }

    public async Task<Appointment> CreateAppointmentAsync(Appointment appointment, CancellationToken cancellationToken)
    {
        await dataContext.Appointments
            .AddAsync(appointment, cancellationToken);

        await dataContext.SaveChangesAsync(cancellationToken);
        return appointment;
    }
    
    public async Task<List<Appointment>> FindAllAppointmentsByDateForManager(DateOnly date, 
        CancellationToken cancellationToken)
    {
        return await dataContext.Appointments
            .Where(a => a.Date == date)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Appointment>> FindAllAppointmentsByDateForPatient(Guid userPatientId, DateOnly date,
        CancellationToken cancellationToken)
    {
        return await dataContext.Appointments
            .Include(a => a.UserPatient)
            .Where(a => a.UserPatient.UserId == userPatientId)
            .Where(a => a.Date == date)
            .ToListAsync(cancellationToken);
    }
    
    public async Task<List<Appointment>> FindAllAppointmentsByDateForDoctor(Guid userDoctorId, DateOnly date, 
        CancellationToken cancellationToken)
    {
        return await dataContext.Appointments
            .Include(a => a.UserDoctor)
            .Where(a => a.UserDoctor.UserId == userDoctorId)
            .Where(a => a.Date == date)
            .ToListAsync(cancellationToken);
    }
}
