using Domain.Abstractions.Errors;
using Domain.Doctors;
using MediatR;

namespace Application.UseCases.Doctors.GetDoctor;

internal sealed class GetDoctorQueryHandler(
    IDoctorRepository doctorRepository
) : IRequestHandler<GetDoctorQuery, Result<DoctorResponse>>
{
    public async Task<Result<DoctorResponse>> Handle(GetDoctorQuery query, CancellationToken ct)
    {
        var doctor = await doctorRepository.FindDoctorByIdAsync(query.DoctorId, ct);

        return doctor is null 
            ? Result.Failure<DoctorResponse>(DoctorErrors.NotFound(query.DoctorId)) 
            : DoctorResponse.FromDoctor(doctor);
    }
}
