using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Operations.AppointSettings.Queries.GetAppointmentSettings;

public class GetConfigAppointmentQueryHandler(IAppointmentSettingsRepository settingsRepository) 
    : IRequestHandler<GetConfigAppointmentQuery, AppointmentSettingsResponse>
{
    public async Task<AppointmentSettingsResponse> 
        Handle(GetConfigAppointmentQuery request, CancellationToken cancellationToken)
    {
        var config = await settingsRepository.FindConfigByIdAsync(request.SettingsId, cancellationToken)
                     ?? throw new NotFoundException(nameof(User), request.SettingsId);

        return new AppointmentSettingsResponse()
            .ToAppointmentSettings(config);
    }
}
