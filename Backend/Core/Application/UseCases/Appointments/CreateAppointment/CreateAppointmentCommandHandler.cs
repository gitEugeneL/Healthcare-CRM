using Domain.Abstractions;
using Domain.Abstractions.Errors;
using Domain.Appointments;
using Domain.Doctors;
using Domain.Patients;
using Domain.WorkSchedules;
using MediatR;

namespace Application.UseCases.Appointments.CreateAppointment;

internal sealed class CreateAppointmentCommandHandler(
    IAppointmentRepository appointmentRepository,
    IDoctorRepository doctorRepository,
    IPatientRepository patientRepository,
    IUnitOfWork unitOfWork   
) : IRequestHandler<CreateAppointmentCommand, Result<AppointmentResponse>>
{
    public async Task<Result<AppointmentResponse>> Handle(CreateAppointmentCommand command, CancellationToken ct)
    {
        var doctor = await doctorRepository.FindActiveDoctorByIdAsync(command.DoctorId, ct);
        if (doctor is null or { WorkSchedule: null })
            return Result.Failure<AppointmentResponse>(DoctorErrors.NotFound(command.DoctorId));
        
        var patient = await patientRepository.FindPatientByIdAsync(command.PatientId, ct);
        if (patient is null)
            return Result.Failure<AppointmentResponse>(PatientErrors.NotFound(command.PatientId));
        
        if (!doctor.WorkSchedule.IsAvailableDay(command.Date.DayOfWeek))
            return Result.Failure<AppointmentResponse>(WorkScheduleErrors.UnavailableWorkday(command.Date.DayOfWeek));
        
        var alreadyBooked = await appointmentRepository.IsPatientAlreadyBookedAsync(
            doctor.Id, patient.Id, command.Date, ct);
        if (alreadyBooked)
            return Result.Failure<AppointmentResponse>(AppointmentErrors.AlreadyBooked);
        
        var slots = await appointmentRepository.GetAvailableSlots(doctor, command.Date, ct);
        if (slots.All(slot => slot.Start != command.StartTime))
            return Result.Failure<AppointmentResponse>(AppointmentErrors.UnavailableSlot);

        Result<Appointment> result = Appointment.Create(
            date: command.Date,
            startTime: command.StartTime,
            endTime: command.StartTime.AddMinutes((int) doctor.WorkSchedule.AppointmentDuration),
            doctorId: doctor.Id,
            patientId: patient.Id);
        
        if (result.IsFailure)
            return Result.Failure<AppointmentResponse>(result.Error);
        
        await appointmentRepository.InsertAppointmentAsync(result.Value, ct);
        await unitOfWork.SaveChangesAsync(ct);
        
        return AppointmentResponse.FromAppointment(result.Value);
    }
}
