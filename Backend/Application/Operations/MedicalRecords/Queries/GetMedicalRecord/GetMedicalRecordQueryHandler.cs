using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Operations.MedicalRecords.Queries.GetMedicalRecord;

public class GetMedicalRecordQueryHandler(IMedicalRecordRepository medicalRecordRepository) 
    : IRequestHandler<GetMedicalRecordQuery, MedicalRecordResponse>
{
    public async Task<MedicalRecordResponse> Handle(GetMedicalRecordQuery request, CancellationToken cancellationToken)
    {
        var medicalRecord = await medicalRecordRepository
                                .FindMedicalRecordByIdAsync(request.MedicalRecordId, cancellationToken)
                            ?? throw new NotFoundException(nameof(MedicalRecords), request.MedicalRecordId);
        
        return request.GetCurrentUserRole() switch
        {
            nameof(Role.Doctor) when medicalRecord.UserDoctor.UserId == request.GetCurrentUserId() =>
                new MedicalRecordResponse()
                    .ToMedicalRecordResponse(medicalRecord),

            nameof(Role.Patient) when medicalRecord.UserPatient.UserId == request.GetCurrentUserId() =>
                new MedicalRecordResponse()
                    .ToMedicalRecordResponse(medicalRecord),

            _ => throw new AccessDeniedException(nameof(MedicalRecord), request.MedicalRecordId)
        };
    }
}
