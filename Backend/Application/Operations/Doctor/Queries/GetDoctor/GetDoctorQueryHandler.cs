using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Operations.Doctor.Queries.GetDoctor;

public class GetDoctorQueryHandler(IDoctorRepository doctorRepository) 
    : IRequestHandler<GetDoctorQuery, DoctorResponse>
{
    public async Task<DoctorResponse> Handle(GetDoctorQuery request, CancellationToken cancellationToken)
    {
        var doctor = await doctorRepository.FindDoctorByUserIdAsync(request.Id, cancellationToken)
                     ?? throw new NotFoundException(nameof(User), request.Id);

        return new DoctorResponse()
            .ToDoctorResponse(doctor);
    }
}
