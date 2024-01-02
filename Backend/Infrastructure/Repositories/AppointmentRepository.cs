using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class AppointmentRepository(DataContext dataContext) : IAppointmentRepository
{
    public async Task<List<DoctorHours>> FindFreeHours(
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
}
