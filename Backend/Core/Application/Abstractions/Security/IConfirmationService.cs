namespace Application.Abstractions.Security;

public interface IConfirmationService
{
    Task<DateTime> GenerateCodeAsync(string email);

    Task<bool> ValidateCodeAsync(string email, string code);
}