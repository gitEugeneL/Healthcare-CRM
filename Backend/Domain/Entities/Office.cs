using Domain.Common;

namespace Domain.Entities;

public class Office : BaseEntity
{
    public required string Name { get; set; }
    public required int Number { get; init; }
    public bool IsAvailable { get; set; }
}
