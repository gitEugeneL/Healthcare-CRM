namespace Application.Common.Models;

public abstract record CurrentUser
{
     private Guid _currentUserId;
     private string? _currentUserRole;
     
     public void SetCurrentUserId(string id) => _currentUserId = Guid.Parse(id);
     public void SerCurrentUserRole(string role) => _currentUserRole = role;
     public Guid GetCurrentUserId() => _currentUserId;
     public string? GetCurrentUserRole() => _currentUserRole;
}
