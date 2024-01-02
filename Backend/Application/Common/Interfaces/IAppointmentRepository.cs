using Application.Common.Models;
using Domain.Entities;

namespace Application.Common.Interfaces;

public interface IAppointmentRepository
{
    Task<List<DoctorHours>> FindFreeHours(UserDoctor doctor, DateOnly date, CancellationToken cancellationToken);
}
