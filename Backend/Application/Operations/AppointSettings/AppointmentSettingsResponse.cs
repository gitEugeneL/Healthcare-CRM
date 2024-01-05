using Domain.Entities;
using Domain.Enums;

namespace Application.Operations.AppointSettings;

public sealed record AppointmentSettingsResponse
{
    public string StartTime { get; set; } = string.Empty;
    public string EndTime { get; set; } = string.Empty;
    public string Interval { get; set; } = string.Empty;
    public List<Workday> Workdays { get; set; } = [];

    public AppointmentSettingsResponse ToAppointmentSettings(AppointmentSettings settings)
    {
        StartTime = settings.StartTime.ToString("HH:mm");
        EndTime = settings.EndTime.ToString("HH:mm");
        Interval = settings.Interval.ToString();
        Workdays = settings.Workdays;

        return this;
    }
}
