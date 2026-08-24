namespace Application.UseCases.Appointments;

public sealed record AppointmentSlotsResponse(
    Guid DoctorId,
    DateOnly Date,
    IReadOnlyList<TimeSlot> TimeSlots
);

public sealed record TimeSlot(TimeOnly Start, TimeOnly End);