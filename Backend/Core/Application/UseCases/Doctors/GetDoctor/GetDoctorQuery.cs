using Domain.Abstractions.Errors;
using MediatR;

namespace Application.UseCases.Doctors.GetDoctor;

public sealed record GetDoctorQuery(Guid DoctorId) : IRequest<Result<DoctorResponse>>;
