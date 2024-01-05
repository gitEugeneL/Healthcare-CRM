using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Operations.MedicalRecords.Commands.CreateMedicalRecord;

public class CreateMedicalRecordCommandHandler(
    IDoctorRepository doctorRepository,
    IPatientRepository patientRepository,
    IAppointmentRepository appointmentRepository,
    IMedicalRecordRepository medicalRecordRepository
    )
    : IRequestHandler<CreateMedicalRecordCommand, MedicalRecordResponse>
{
    public async Task<MedicalRecordResponse> Handle(CreateMedicalRecordCommand request, CancellationToken cancellationToken)
    {
        var doctor = await doctorRepository.FindDoctorByUserIdAsync(request.GetCurrentUserId(), cancellationToken)
                     ?? throw new NotFoundException(nameof(User), request.GetCurrentUserId());

        var patient = await patientRepository.FindPatientByUserIdAsync(request.UserPatientId, cancellationToken)
                      ?? throw new NotFoundException(nameof(User), request.UserPatientId);

        var appointment = await appointmentRepository.FindAppointmentByIdAsync(request.AppointmentId, cancellationToken)
                          ?? throw new NotFoundException(nameof(Appointment), request.AppointmentId);

        if (appointment.UserDoctor.UserId != request.GetCurrentUserId())
            throw new UnauthorizedException($"Doctor doesn't have access to ({appointment.Id}) appointment");

        if (!appointment.IsCompleted)
            throw new UnauthorizedException($"({appointment.Id}) appointment has not yet been completed");     
        
        if (appointment.UserPatient.UserId != request.UserPatientId)
            throw new NotFoundException(nameof(User), request.UserPatientId);
        
        var medicalRecord = await medicalRecordRepository
            .FindMedicalRecordByIdAsync(request.AppointmentId, cancellationToken);
        if (medicalRecord is not null)
            throw new AlreadyExistException(nameof(MedicalRecord), medicalRecord.Id);
        
        medicalRecord = await medicalRecordRepository.CreateMedicalRecordAsync(
            new MedicalRecord
            {
                Title = request.Title,
                DoctorNote = request.DoctorNote,
                UserPatient = patient,
                UserDoctor = doctor,
                Appointment = appointment
            },
            cancellationToken
        );
        
        return new MedicalRecordResponse()
            .ToMedicalRecordResponse(medicalRecord);
    }
}
