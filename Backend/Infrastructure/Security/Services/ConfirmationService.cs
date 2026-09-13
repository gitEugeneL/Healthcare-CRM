using Application.Abstractions.Cache;
using Application.Abstractions.Mail;
using Application.Abstractions.Security;
using Microsoft.Extensions.Configuration;

namespace Security.Services;


internal sealed class ConfirmationService(
    IConfiguration configuration, 
    ICacheService cache,
    IMailService messageService) : IConfirmationService
{
    private static string CodeKey(string email) => $"confirmation:code:{email}";

    
    private readonly int _codeLength = int.Parse(configuration["Authentication:ConfirmationCode:Length"] ?? 
                                                 throw new ApplicationException("CodeLength not found in config"));

    private readonly int _lifetimeMinutes = int.Parse(configuration["Authentication:ConfirmationCode:LifetimeInMinutes"] ??
                                                      throw new ApplicationException("Lifetime not found in config"));
    
    public async Task<DateTime> GenerateCodeAsync(string email)
    {
        var code = Random.Shared.Next((int)Math.Pow(10, _codeLength - 1), (int)Math.Pow(10, _codeLength)).ToString();
        var lifetime = TimeSpan.FromMinutes(_lifetimeMinutes);
        var expires = DateTime.UtcNow.Add(lifetime);
        
        await cache.SetAsync(CodeKey(email), code, lifetime);
        await messageService.SendMessageAsync(email, "Confirmation code", code, expires);
        
        return expires;
    }
    
    public async Task<bool> ValidateCodeAsync(string email, string code)
    {
        var storedCode = await cache.GetAsync<string>(CodeKey(email));
        var isValid = storedCode == code;
        
        if (isValid)
            await cache.RemoveAsync(CodeKey(email));
        
        return isValid;
    }
}