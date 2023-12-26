using MediatR;

namespace Application.Operations.Patients.Commands.DeletePatient;

public class DeletePatientCommand : IRequest<Unit>
{
    public Guid CurrentUserId { get; private set; }
    
    public DeletePatientCommand SetCurrentUserId(string id)
    {
        CurrentUserId = Guid.Parse(id);
        return this;
    }
}
