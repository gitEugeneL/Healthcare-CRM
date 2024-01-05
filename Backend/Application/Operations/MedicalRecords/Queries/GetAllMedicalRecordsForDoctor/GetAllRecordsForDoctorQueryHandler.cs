using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using MediatR;

namespace Application.Operations.MedicalRecords.Queries.GetAllMedicalRecordsForDoctor;

public class GetAllRecordsForDoctorQueryHandler(
    IMedicalRecordRepository medicalRecordRepository,
    IDoctorRepository doctorRepository
    ) : IRequestHandler<GetAllRecordsForDoctorQueryPagination, PaginatedList<MedicalRecordResponse>>
{
    public async Task<PaginatedList<MedicalRecordResponse>> 
        Handle(GetAllRecordsForDoctorQueryPagination request, CancellationToken cancellationToken)
    {
        var doctor = await doctorRepository.FindDoctorByUserIdAsync(request.GetCurrentUserId(), cancellationToken)
                     ?? throw new NotFoundException(nameof(User), request.GetCurrentUserId());

        var (medicalRecords, count) = await medicalRecordRepository
            .GetMedicalRecordsForDoctorWithPaginationAsync(
                cancellationToken: cancellationToken,
                pageNumber: request.PageNumber,
                pageSize: request.PageSize,
                doctorId: doctor.User.Id,
                patientId: request.UserPatientId,
                sortByDate: request.SortByDate,
                sortOrderAsc: request.SortOrderAsc
            );
        
        var medicalRecordResponses = medicalRecords
            .Select(mr => new MedicalRecordResponse()
                .ToMedicalRecordResponse(mr))
            .ToList();
        
        return new PaginatedList<MedicalRecordResponse>(
            medicalRecordResponses, count, request.PageNumber, request.PageSize);
    }
}
