using Application.Abstractions.Security;
using Application.Abstractions.Security.Models;
using Microsoft.Extensions.Configuration;

namespace Security.Services;

internal sealed class LockoutService(IConfiguration configuration, AttemptLimiter limiter) : ILockoutService
{
    private static string LoginKey(string email) => $"lockout:login/refresh/logout:{email.Trim().ToLowerInvariant()}";
    private static string GenerateCodeKey(string email) => $"lockout:confirm:generate:{email.Trim().ToLowerInvariant()}";
    private static string ConfirmFailKey(string email) => $"lockout:confirm:failed:{email.Trim().ToLowerInvariant()}";
    
    private readonly int _loginMaxAttempts = int.Parse(configuration["Authentication:LoginLockout:MaxAttempts"] ?? 
                                                       throw new ApplicationException("MaxAttempts not found in config"));
   
    private readonly TimeSpan _loginWindow =
        TimeSpan.FromMinutes(int.Parse(configuration["Authentication:LoginLockout:LifetimeInMinutes"] ??
                                       throw new ApplicationException("MaxAttempts not found in config")) );
        
    private readonly int _confirmationCodeMaxAttempts = int.Parse(configuration["Authentication:ConfirmationCode:MaxAttempts"] 
                                                                  ?? throw new ApplicationException("MaxAttempts not found in config"));
    
    private readonly int _confirmMaxAttempts = int.Parse(configuration["Authentication:ConfirmLockout:MaxAttempts"] ?? 
                                               throw new ApplicationException("MaxAttempts not found in config"));
    
    private readonly TimeSpan _confirmWindow = TimeSpan.FromMinutes(int.Parse(configuration["Authentication:ConfirmLockout:LifetimeInMinutes"] ??
                                                                              throw new ApplicationException("MaxAttempts not found in config")));
    
    public Task<AttemptState> CheckLoginLockoutAsync(string email, CancellationToken ct = default) =>
        limiter.GetStateAsync(LoginKey(email));

    public Task<AttemptState> CheckGenerateCodeLockoutState(string email, CancellationToken ct = default) =>
        limiter.GetStateAsync(GenerateCodeKey(email));

    public Task<AttemptState> CheckConfirmLockoutState(string email, CancellationToken ct) => 
    limiter.GetStateAsync(ConfirmFailKey(email));

    public Task<AttemptState> ProcessForLoginAsync(string email, bool isPasswordValid, CancellationToken ct = default) =>
        limiter.RegisterAttemptAsync(LoginKey(email), _loginMaxAttempts, _loginWindow, isPasswordValid);

    public Task<AttemptState> ProcessForGenerateCodeAsync(string email, CancellationToken ct = default) =>
        limiter.RegisterAttemptAsync(GenerateCodeKey(email), _confirmationCodeMaxAttempts, _confirmWindow, success: false);

    public Task<AttemptState> ProcessForConfirmAsync(string email, bool isCodeValid, CancellationToken ct = default) =>
        limiter.RegisterAttemptAsync(ConfirmFailKey(email), _confirmMaxAttempts, _confirmWindow, isCodeValid);
}