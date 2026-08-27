using Domain.Abstractions;
using Domain.Abstractions.Errors;
using Domain.Appointments;
using Domain.MedicalRecords;
using MediatR;

namespace Application.UseCases.MedicalRecords.CreateMedicalRecord;

internal sealed class CreateMedicalRecordCommandHandler(
    IAppointmentRepository appointmentRepository,
    IMedicalRecordRepository medicalRecordRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<CreateMedicalRecordCommand, Result<MedicalRecordResponse>>
{
    public async Task<Result<MedicalRecordResponse>> Handle(CreateMedicalRecordCommand command, CancellationToken ct)
    {
        var appointment = await appointmentRepository.FindAppointmentByIdWithTrackingAsync(command.AppointmentId, ct);
      
        if (appointment is null)
            return Result.Failure<MedicalRecordResponse>(AppointmentErrors.NotFound(command.AppointmentId));
        
        if (appointment.Status == AppointmentStatus.Canceled)
            return Result.Failure<MedicalRecordResponse>(AppointmentErrors.AlreadyCanceled);
        
        if (await medicalRecordRepository.IsMedicalRecordByAppointmentIdExistsAsync(command.AppointmentId, ct))
            return Result.Failure<MedicalRecordResponse>(MedicalRecordErrors.AlreadyExist(command.AppointmentId));
        
        Result<MedicalRecord> medicalRecord = MedicalRecord.Create(
            appointmentId: command.AppointmentId, 
            title: command.Title, 
            doctorNote: command.DoctorNote, 
            recommendationForPatient: command.RecommendationForPatient, 
            diagnosis: command.Diagnosis, 
            icdCode: command.IcdCode, 
            followUpDate: command.FollowUpDate);
        
        if (medicalRecord.IsFailure)
            return Result.Failure<MedicalRecordResponse>(medicalRecord.Error);
        
        appointment.Complete();
        
        await medicalRecordRepository.InsertMedicalRecordAsync(medicalRecord.Value, ct);
        
        await unitOfWork.SaveChangesAsync(ct);

        return MedicalRecordResponse.FromMedicalRecord(medicalRecord.Value);
    }
}
