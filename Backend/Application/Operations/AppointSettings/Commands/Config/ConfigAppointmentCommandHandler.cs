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
        
        config.Interval = request.Interval switch
        {
            "H1" => Interval.H1,
            "M15" => Interval.M15,
            "M30" => Interval.M30,
            _ => config.Interval
        };
        
        var workdays = new List<Workday>();
        
        if (request.Monday) 
            workdays.Add(Workday.Monday);
        if (request.Tuesday) 
            workdays.Add(Workday.Tuesday);
        if (request.Wednesday) 
            workdays.Add(Workday.Wednesday);
        if (request.Thursday) 
            workdays.Add(Workday.Thursday);
        if (request.Friday) 
            workdays.Add(Workday.Friday);
        if (request.Saturday) 
            workdays.Add(Workday.Saturday);
        if (request.Sunday) 
            workdays.Add(Workday.Sunday);
        
        config.Workdays = workdays;

        var updatedConfig = await settingsRepository.UpdateConfigAsync(config, cancellationToken);
        return new AppointmentSettingsResponse()
            .ToAppointmentSettings(updatedConfig);
    }
}
