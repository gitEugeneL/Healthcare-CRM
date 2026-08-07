using Domain.Abstractions.Errors;

namespace Domain.WorkSchedules;

public static class WorkScheduleErrors
{
    public static Error NotFound(Guid workScheduleId) => Error.NotFound(
        "WorkSchedule.NotFound", 
        $"The WorkSchedule with the identifier {workScheduleId} was not found");
    
    public static readonly Error InvalidTimeRange = Error.Problem(
        "WorkSchedule.InvalidTimeRange",
        "Start time must be earlier than end time");

    public static readonly Error EmptyWorkdays = Error.Problem(
        "WorkSchedule.EmptyWorkdays",
        "At least one workday must be specified");

    public static readonly Error DuplicateWorkdays = Error.Conflict(
        "WorkSchedule.DuplicateWorkdays",
        "Workdays must not contain duplicates");
    
}
