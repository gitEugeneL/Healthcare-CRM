using Domain.Abstractions.Errors;
using MediatR;

namespace Application.UseCases.Security.GenerateCode;

public sealed record GenerateCodeCommand(string Email) : IRequest<Result<GenerateCodeResponse>>;