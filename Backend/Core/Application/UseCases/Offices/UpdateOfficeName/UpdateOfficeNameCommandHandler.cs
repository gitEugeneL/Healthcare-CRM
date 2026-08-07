using Domain.Abstractions;
using Domain.Abstractions.Errors;
using Domain.Offices;
using MediatR;

namespace Application.UseCases.Offices.UpdateOfficeName;

internal sealed class UpdateOfficeNameCommandHandler(
    IOfficeRepository officeRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<UpdateOfficeNameCommand, Result<OfficeResponse>>
{
    public async Task<Result<OfficeResponse>> Handle(UpdateOfficeNameCommand command, CancellationToken ct)
    {
        var office = await officeRepository.FindOfficeByIdAsync(command.OfficeId, ct);
        if (office is null)
        {
            return Result.Failure<OfficeResponse>(OfficeErrors.NotFound(command.OfficeId));
        }
        
        office.ChangeName(command.Name);
        
        await unitOfWork.SaveChangesAsync(ct);
        
        return OfficeResponse.FromOffice(office);
    }
}