using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Operations.MedicalRecords.Commands.UpdateMedicalRecord;

public class UpdateMedicalRecordCommandHandler(IMedicalRecordRepository medicalRecordRepository) 
    : IRequestHandler<UpdateMedicalRecordCommand, MedicalRecordResponse>
{
    public async Task<MedicalRecordResponse> Handle(UpdateMedicalRecordCommand request, CancellationToken cancellationToken)
    {
        var medicalRecord = await medicalRecordRepository
                                .FindMedicalRecordByIdAsync(request.MedicalRecordId, cancellationToken)
                            ?? throw new NotFoundException(nameof(MedicalRecord), request.MedicalRecordId);
        
        if(medicalRecord.UserDoctor.UserId != request.GetCurrentUserId())
            throw new UnauthorizedException($"Doctor doesn't have access to {medicalRecord.Id} medical record");

        medicalRecord.Title = request.Tittle ?? medicalRecord.Title;
        medicalRecord.DoctorNote = request.DoctorNote ?? medicalRecord.DoctorNote;

        var updatedMedicalRecord = await medicalRecordRepository
            .UpdateMedicalRecordAsync(medicalRecord, cancellationToken);

        return new MedicalRecordResponse()
            .ToMedicalRecordResponse(updatedMedicalRecord);
    }
}
