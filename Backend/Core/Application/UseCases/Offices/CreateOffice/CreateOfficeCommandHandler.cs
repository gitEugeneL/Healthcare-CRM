using Domain.Abstractions;
using Domain.Abstractions.Errors;
using Domain.Offices;
using MediatR;

namespace Application.UseCases.Offices.CreateOffice;

internal sealed class CreateOfficeCommandHandler(
    IOfficeRepository officeRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<CreateOfficeCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateOfficeCommand command, CancellationToken ct)
    {
        if (await officeRepository.OfficeExistsByNumberAsync(command.Number, ct))
        {
            return Result.Failure<Guid>(OfficeErrors.AlreadyExist(command.Number));
        }
        var office = Office.Create(command.Name, command.Number);
        
        await officeRepository.InsertOfficeAsync(office, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return office.Id;
    }
}
