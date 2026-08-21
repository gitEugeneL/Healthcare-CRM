using Domain.Abstractions.Errors;
using MediatR;

namespace Application.UseCases.Specializations.GetAllSpecializations;

public sealed record GetAllSpecializationQuery : IRequest<Result<IReadOnlyList<SpecializationResponse>>>;
