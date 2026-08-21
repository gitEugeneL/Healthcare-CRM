namespace Application.UseCases.Patients;

public sealed record PatientAddress(
    string Province,
    string PostalCode,
    string City,
    string Street,
    string Hose,
    string? Apartment
);