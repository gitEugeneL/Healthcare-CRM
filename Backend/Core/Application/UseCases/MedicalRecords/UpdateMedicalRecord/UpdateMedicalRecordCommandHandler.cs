using Domain.Abstractions;
using Domain.Abstractions.Errors;
using Domain.MedicalRecords;
using MediatR;

namespace Application.UseCases.MedicalRecords.UpdateMedicalRecord;

internal sealed class UpdateMedicalRecordCommandHandler(
    IMedicalRecordRepository medicalRecordRepository,
    IUnitOfWork unitOfWork   
) : IRequestHandler<UpdateMedicalRecordCommand, Result<MedicalRecordResponse>> 
{
    public async Task<Result<MedicalRecordResponse>> Handle(UpdateMedicalRecordCommand command, CancellationToken ct)
    {
        var medicalRecord = await medicalRecordRepository
            .FindMedicalRecordByIdWithTrackingAsync(command.MedicalRecordId, ct);

        if (medicalRecord is null)
            return Result.Failure<MedicalRecordResponse>(MedicalRecordErrors.NotFound(command.MedicalRecordId));

        if (command.Title is not null)
            medicalRecord.ChangeTittle(command.Title);
        
        if (command.RecommendationForPatient is not null)
            medicalRecord.ChangeRecommendationForPatient(command.RecommendationForPatient);

        if (command.FollowUpDate is not null)
        {
            var result = medicalRecord.ChangeFollowUpDate(command.FollowUpDate.Value);
            if (result.IsFailure)
                return Result.Failure<MedicalRecordResponse>(result.Error);
        }

        await unitOfWork.SaveChangesAsync(ct);

        return MedicalRecordResponse.FromMedicalRecord(medicalRecord);
        
    }
}
