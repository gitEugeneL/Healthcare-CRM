using Domain.Entities;

namespace Application.Common.Interfaces;

public interface IDoctorRepository
{
    Task<UserDoctor> CreateDoctorAsync(UserDoctor doctor, CancellationToken cancellationToken);

    Task<UserDoctor> UpdateDoctorAsync(UserDoctor doctor, CancellationToken cancellationToken);

    Task<UserDoctor?> FindDoctorByUserIdAsync(Guid id, CancellationToken cancellationToken);
}