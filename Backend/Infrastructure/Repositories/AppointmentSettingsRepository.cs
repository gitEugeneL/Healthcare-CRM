using Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

internal class AppointmentSettingsRepository(DataContext dataContext) : IAppointmentSettingsRepository
{
    public async Task<AppointmentSettings> 
        UpdateConfigAsync(AppointmentSettings config, CancellationToken cancellationToken)
    {
        dataContext.AppointmentSettings.Update(config);
        await dataContext.SaveChangesAsync(cancellationToken);
        return config;
    }
    
    public async Task<AppointmentSettings?> FindConfigByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await dataContext.AppointmentSettings
            .FirstOrDefaultAsync(settings => settings.Id == id, cancellationToken);
    }
}
