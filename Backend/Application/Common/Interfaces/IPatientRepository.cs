using Domain.Entities;

namespace Application.Common.Interfaces;

public interface IPatientRepository
{
    Task<UserPatient> CreatePatientAsync(UserPatient patient, CancellationToken cancellationToken);

    Task<UserPatient> UpdatePatientAsync(UserPatient patient, CancellationToken cancellationToken);

    Task DeletePatientAsync(UserPatient patient, CancellationToken cancellationToken);
    
    Task<UserPatient?> FindPatientByUserIdAsync(Guid id, CancellationToken cancellationToken);
    
    Task<(IEnumerable<UserPatient> List, int Count)> GetPatientsWithPaginationAsync(
        CancellationToken cancellationToken, int pageNumber, int pageSize);
}
