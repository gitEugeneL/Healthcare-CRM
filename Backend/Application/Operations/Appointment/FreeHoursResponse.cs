using Application.Common.Models;
using Domain.Entities;

namespace Application.Operations.Appointment;

public sealed record FreeHoursResponse
{
    public Guid UserDoctorId { get; set; }
    public List<DoctorHours> FreeHours { get; set; } = [];

    public FreeHoursResponse ToFreeHoursResponse(UserDoctor doctor, List<DoctorHours> freeHours)
    {
        UserDoctorId = doctor.UserId;
        FreeHours = freeHours;
        
        return this;
    }
}
