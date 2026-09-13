
using Application.Abstractions.Security.Models;

namespace Application.Abstractions.Security;

public interface ILockoutService
{
    Task<AttemptState> CheckLoginLockoutAsync(string email, CancellationToken ct);
    
    Task<AttemptState> CheckGenerateCodeLockoutState(string email, CancellationToken ct);
    
    Task<AttemptState> CheckConfirmLockoutState(string email, CancellationToken ct);
    
    Task<AttemptState> ProcessForLoginAsync(string email, bool isPasswordValid, CancellationToken ct);
    
    Task<AttemptState> ProcessForGenerateCodeAsync(string email, CancellationToken ct);
    
    Task<AttemptState> ProcessForConfirmAsync(string email, bool isCodeValid, CancellationToken ct);
}