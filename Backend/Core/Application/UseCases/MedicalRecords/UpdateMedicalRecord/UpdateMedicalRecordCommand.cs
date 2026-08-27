using Domain.Abstractions.Errors;
using MediatR;

namespace Application.UseCases.MedicalRecords.UpdateMedicalRecord;

public sealed record UpdateMedicalRecordCommand(
    Guid MedicalRecordId,
    string? Title,
    string? RecommendationForPatient,
    DateOnly? FollowUpDate
) : IRequest<Result<MedicalRecordResponse>>;