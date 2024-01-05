using Domain.Common;

namespace Domain.Entities;

public class Office : BaseEntity
{
    public required string Name { get; set; }
    public required ushort Number { get; init; }
    public bool IsAvailable { get; set; }
}
