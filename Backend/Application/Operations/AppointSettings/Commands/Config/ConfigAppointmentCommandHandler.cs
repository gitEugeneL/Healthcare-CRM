using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Operations.AppointSettings.Commands.Config;

public class ConfigAppointmentCommandHandler(
    IAppointmentSettingsRepository settingsRepository,
    IDoctorRepository doctorRepository
    )
    : IRequestHandler<ConfigAppointmentCommand, AppointmentSettingsResponse>
{
    public async Task<AppointmentSettingsResponse> 
        Handle(ConfigAppointmentCommand request, CancellationToken cancellationToken)
    {
        var doctor = await doctorRepository.FindDoctorByUserIdAsync(request.CurrentUserId, cancellationToken)
                     ?? throw new NotFoundException(nameof(User), request.CurrentUserId);

        var config = doctor.AppointmentSettings;

        var startTime = request.StartTime is not null ? TimeOnly.Parse(request.StartTime) : config.StartTime;
        var endTime = request.EndTime is not null ? TimeOnly.Parse(request.EndTime) : config.EndTime;

        if (startTime >= endTime)
            throw new TimeMismatchException("EndTime must be greater than StartTime");

        config.StartTime = startTime;
        config.EndTime = endTime;

        if (request.Workdays is not null)
        {
            var workdays = Enum.GetValues(typeof(Workday)).Cast<Workday>()
                .Where(day => request.Workdays.Contains((int) day))
                .ToList();
            config.Workdays = workdays;
        }
        
        config.Interval = request.Interval switch
        {
            "60min" => Interval.Min60,
            "15min" => Interval.Min15,
            "30min" => Interval.Min30,
            _ => config.Interval
        };
        
        var updatedConfig = await settingsRepository.UpdateConfigAsync(config, cancellationToken);
        return new AppointmentSettingsResponse()
            .ToAppointmentSettings(updatedConfig);
    }
}
