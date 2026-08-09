using Domain.Abstractions.Errors;
using MediatR;

namespace Application.UseCases.Doctors.ChangeDoctorStatus;

public sealed record ChangeDoctorStatusCommand(Guid DoctorId) : IRequest<Result<DoctorResponse>>;