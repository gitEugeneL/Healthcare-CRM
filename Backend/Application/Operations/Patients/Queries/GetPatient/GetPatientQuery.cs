using MediatR;

namespace Application.Operations.Patients.Queries.GetPatient;

public sealed record GetPatientQuery(Guid Id) : IRequest<PatientResponse>;
