using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Operations.Appointments.Commands.CreateAppointment;

public class CreateAppointmentCommandHandler(
    IDoctorRepository doctorRepository,
    IPatientRepository patientRepository,
    IAppointmentRepository appointmentRepository
    ) 
    : IRequestHandler<CreateAppointmentCommand, AppointmentResponse>
{
    public async Task<AppointmentResponse> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
    {
        var patient = await patientRepository.FindPatientByUserIdAsync(request.GetCurrentUserId(), cancellationToken)
                      ?? throw new NotFoundException(nameof(User), request.GetCurrentUserId());
        
        var doctor = await doctorRepository.FindDoctorByUserIdAsync(request.UserDoctorId, cancellationToken)
                     ?? throw new NotFoundException(nameof(User), request.UserDoctorId);
        
        var date = DateOnly.Parse(request.Date);
        var startTime = TimeOnly.Parse(request.StartTime);
        var endTime = startTime.AddMinutes((double) doctor.AppointmentSettings.Interval);

        // check doctor's workdays
        if (!doctor.AppointmentSettings.Workdays.Contains((Workday) date.DayOfWeek))
            throw new NotFoundException(nameof(User), request.UserDoctorId + " doesn't work on this day");

        // check doctor's free hours
        var freeDoctorHours = await appointmentRepository
            .FindFreeHoursAsync(doctor, date, cancellationToken);
        if (freeDoctorHours.All(freeTime => freeTime.Start != startTime))
            throw new NotFoundException(nameof(User), request.UserDoctorId + " isn't available at this time");

        var appointment = await appointmentRepository.CreateAppointmentAsync(
            new Appointment
            {
                Date = date,
                StartTime = startTime,
                EndTime = endTime,
                IsCanceled = false,
                IsCompleted = false,
                UserPatient = patient,
                UserDoctor = doctor
            },
            cancellationToken
        );

        return new AppointmentResponse()
            .ToAppointmentResponse(appointment);
    }
}
