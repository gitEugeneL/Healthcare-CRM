using Domain.Common;

namespace Domain.Entities;

public sealed class Specialization : BaseEntity
{
    public required string Value { get; init; }
    public string? Description { get; set; }
    
    /*** Relations ***/
    public List<UserDoctor> UserDoctors { get; init; } = [];
}
