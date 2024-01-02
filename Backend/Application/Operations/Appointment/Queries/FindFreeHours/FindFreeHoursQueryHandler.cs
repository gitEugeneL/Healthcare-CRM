using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Operations.Appointment.Queries.FindFreeHours;

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
        
        if (!CheckDoctorWorkdays(date, doctor.AppointmentSettings.Workdays)) 
            throw new NotFoundException(nameof(User), request.UserDoctorId + " doesn't work on this day");

        var result = await appointmentRepository.FindFreeHours(doctor, date, cancellationToken);
        return new FreeHoursResponse()
            .ToFreeHoursResponse(doctor, result);
    }
    
    // todo create service
    private bool CheckDoctorWorkdays(DateOnly date, List<Workday> workdays)
    {
        var day = date.DayOfWeek;
        return workdays.Contains((Workday) day);
    }
}
