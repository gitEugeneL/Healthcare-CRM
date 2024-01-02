using Application.Common.Models;
using Domain.Entities;

namespace Application.Common.Interfaces;

public interface IAppointmentRepository
{
    Task<List<DoctorHours>> FindFreeHoursAsync(UserDoctor doctor, DateOnly date, CancellationToken cancellationToken);

    Task<Appointment> CreateAppointmentAsync(Appointment appointment, CancellationToken cancellationToken);

    Task<List<Appointment>> FindAllAppointmentsByDateForManager(DateOnly date, CancellationToken cancellationToken);

    Task<List<Appointment>> FindAllAppointmentsByDateForPatient(Guid userPatientId, DateOnly date,
        CancellationToken cancellationToken);

    Task<List<Appointment>> FindAllAppointmentsByDateForDoctor(Guid userDoctorId, DateOnly date,
        CancellationToken cancellationToken);
}
