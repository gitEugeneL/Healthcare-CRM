namespace Application.Common.Models;

public abstract record CurrentUser
{
    public Guid CurrentUserId { get; private set; }
    public void SetCurrentUserId(string id) => CurrentUserId = Guid.Parse(id);
}
