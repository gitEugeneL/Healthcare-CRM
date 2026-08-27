using Domain.Abstractions.Errors;
using MediatR;

namespace Application.UseCases.MedicalRecords.CreateMedicalRecord;

public sealed record CreateMedicalRecordCommand(
    Guid AppointmentId,
    string Title,
    string DoctorNote,
    string RecommendationForPatient,
    string Diagnosis,
    string IcdCode,
    DateOnly? FollowUpDate) : IRequest<Result<MedicalRecordResponse>>;
