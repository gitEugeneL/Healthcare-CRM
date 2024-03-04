using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Operations.Appointments.Validations;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Operations.Appointments.Queries.GetAllByDate;

public class GetAllByDateQueryHandler(
    IAppointmentRepository appointmentRepository,
    IUserRepository userRepository
    ) 
    : IRequestHandler<GetAllByDateQuery, List<AppointmentResponse>>
{
    public async Task<List<AppointmentResponse>> Handle(GetAllByDateQuery request, CancellationToken cancellationToken)
    {
        var user = await userRepository.FindUserByIdAsync(request.GetCurrentUserId(), cancellationToken)
                   ?? throw new NotFoundException(nameof(User), request.GetCurrentUserId());
        
        var isValidDate = DateValidatorAttribute.BeValidDate(request.Date);
        if (!isValidDate)
            throw new UnprocessableException("invalid date");
        var date = DateOnly.Parse(request.Date);
        
        var result = request.GetCurrentUserRole() switch
        {
            nameof(Role.Doctor) when user.Role == Role.Doctor =>
                await appointmentRepository
                    .FindAllAppointmentsByDateForDoctor(request.GetCurrentUserId(), date, cancellationToken),
          
            nameof(Role.Patient) when user.Role == Role.Patient =>
                await appointmentRepository
                    .FindAllAppointmentsByDateForPatient(request.GetCurrentUserId(), date, cancellationToken),
          
            nameof(Role.Manager) when user.Role == Role.Manager =>
                await appointmentRepository
                    .FindAllAppointmentsByDateForManager(date, cancellationToken),
            
            _ => throw new NotFoundException(nameof(User), request.GetCurrentUserId())
        };
        
        return result
            .Select(appointment => new AppointmentResponse().ToAppointmentResponse(appointment))
            .ToList();
    }
}
