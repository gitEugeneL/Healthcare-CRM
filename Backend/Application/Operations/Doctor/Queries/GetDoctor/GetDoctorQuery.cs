using MediatR;

namespace Application.Operations.Doctor.Queries.GetDoctor;

public sealed record GetDoctorQuery(Guid Id) : IRequest<DoctorResponse>;

