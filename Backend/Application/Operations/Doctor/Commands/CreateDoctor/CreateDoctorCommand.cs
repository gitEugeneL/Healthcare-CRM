using Application.Operations.Common.Users.Commands;
using MediatR;

namespace Application.Operations.Doctor.Commands.CreateDoctor;

public sealed record CreateDoctorCommand : CreateUserCommand, IRequest<DoctorResponse>;
