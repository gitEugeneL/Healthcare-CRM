namespace Application.Common.Models;

public sealed record DoctorHours(
    TimeOnly Start,
    TimeOnly End
);
    