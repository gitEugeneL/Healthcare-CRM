using Domain.Entities;

namespace Application.Common.Interfaces;

public interface IAppointmentSettingsRepository
{
    Task<AppointmentSettings> UpdateConfigAsync(AppointmentSettings config, CancellationToken cancellationToken);

    Task<AppointmentSettings?> FindConfigByIdAsync(Guid id, CancellationToken cancellationToken);
}
