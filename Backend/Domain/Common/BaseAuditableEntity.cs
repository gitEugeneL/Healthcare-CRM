namespace Domain.Common;

public abstract class BaseAuditableEntity : BaseEntity
{
    public DateTime Created { get; private init; } = DateTime.UtcNow;
    public DateTime? Updated { get; private set; }
    
    public void Update() => Updated = DateTime.UtcNow;
}
