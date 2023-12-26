using Application.Operations.Users.Commands;
using MediatR;

namespace Application.Operations.Doctor.Commands.CreateDoctor;

public sealed record CreateDoctorCommand : CreateUserCommand, IRequest<DoctorResponse>;
