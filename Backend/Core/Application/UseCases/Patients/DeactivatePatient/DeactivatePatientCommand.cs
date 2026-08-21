using Domain.Abstractions.Errors;
using MediatR;

namespace Application.UseCases.Patients.DeactivatePatient;

public sealed record DeactivatePatientCommand(Guid PatientId) : IRequest<Result<Unit>>;