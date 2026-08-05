using Domain.Abstractions.Errors;
using MediatR;

namespace Application.UseCases.Offices.CreateOffice;

public sealed record CreateOfficeCommand(string Name, int Number ) : IRequest<Result<Guid>>;