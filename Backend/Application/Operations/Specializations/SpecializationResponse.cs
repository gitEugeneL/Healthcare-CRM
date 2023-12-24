using Domain.Entities;

namespace Application.Operations.Specializations;

public sealed record SpecializationResponse
{
    public Guid SpecializationId { get; set; }
    public string Value { get; set; } = string.Empty;
    public string? Description { get; set; }

    public SpecializationResponse ToSpecializationResponse(Specialization specialization)
    {
        SpecializationId = specialization.Id;
        Value = specialization.Value;
        Description = specialization.Description;

        return this;
    }
}
