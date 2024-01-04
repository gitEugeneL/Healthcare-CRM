using Application.Common.Models;
using MediatR;

namespace Application.Operations.MedicalRecords.Queries.GetMedicalRecord;

public sealed record GetMedicalRecordQuery(
    Guid MedicalRecordId
) : CurrentUser, IRequest<MedicalRecordResponse>;