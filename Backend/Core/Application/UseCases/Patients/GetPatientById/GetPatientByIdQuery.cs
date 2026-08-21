using Domain.Abstractions.Errors;
using MediatR;

namespace Application.UseCases.Patients.GetPatientById;

public sealed record GetPatientByIdQuery(Guid PatientId) : IRequest<Result<PatientResponse>>;