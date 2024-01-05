using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Operations.Appointments.Queries.FindFreeHours;

public class FindFreeHoursQueryHandler(
    IAppointmentRepository appointmentRepository,
    IDoctorRepository doctorRepository
    )
    : IRequestHandler<FindFreeHoursQuery, FreeHoursResponse>
{
    public async Task<FreeHoursResponse> Handle(FindFreeHoursQuery request, CancellationToken cancellationToken)
    {
        var doctor = await doctorRepository.FindDoctorByUserIdAsync(request.UserDoctorId, cancellationToken)
                     ?? throw new NotFoundException(nameof(Users), request.UserDoctorId);

        var date = DateOnly.Parse(request.Date);
        
        if (!doctor.AppointmentSettings.Workdays.Contains((Workday) date.DayOfWeek))
            throw new NotFoundException(nameof(User), request.UserDoctorId + " doesn't work on this day");
        
        var result = await appointmentRepository.FindFreeHoursAsync(doctor, date, cancellationToken);
        return new FreeHoursResponse()
            .ToFreeHoursResponse(doctor, result);
    }
}
