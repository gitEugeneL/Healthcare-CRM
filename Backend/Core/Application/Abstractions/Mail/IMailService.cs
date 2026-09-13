namespace Application.Abstractions.Mail;

public interface IMailService
{
    public Task<bool> SendMessageAsync(string to, string subject, string body, DateTime expires);
}