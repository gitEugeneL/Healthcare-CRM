using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public class AppointmentSettings : BaseEntity
{
    public required TimeOnly StartTime { get; set; }
    public required TimeOnly EndTime { get; set; }
    public required Interval Interval { get; set; }
    public required List<Workday> Workdays { get; set; }

    /*** Relations ***/
    public UserDoctor? UserDoctor { get; init; }
}
