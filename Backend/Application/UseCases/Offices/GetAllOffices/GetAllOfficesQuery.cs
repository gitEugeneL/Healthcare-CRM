using Domain.Abstractions.Errors;
using MediatR;

namespace Application.UseCases.Offices.GetAllOffices;

public sealed record GetAllOfficesQuery : IRequest<Result<IReadOnlyList<OfficeResponse>>>;