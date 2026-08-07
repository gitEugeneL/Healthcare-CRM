using Domain.Abstractions;
using Domain.Abstractions.Errors;
using Domain.Offices;
using MediatR;

namespace Application.UseCases.Offices.ChangeOfficeStatus;

internal sealed class ChangeOfficeStatusCommandHandler(
    IOfficeRepository officeRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<ChangeOfficeStatusCommand, Result<OfficeResponse>>
{
    public async Task<Result<OfficeResponse>> Handle(ChangeOfficeStatusCommand command, CancellationToken ct)
    {
        var office = await officeRepository.FindOfficeByIdAsync(command.OfficeId, ct);
        if (office is null)
        {
            return Result.Failure<OfficeResponse>(OfficeErrors.NotFound(command.OfficeId));
        }
        
        office.ChangeAvailability();
        
        await unitOfWork.SaveChangesAsync(ct);
        
        return OfficeResponse.FromOffice(office);
    }
}