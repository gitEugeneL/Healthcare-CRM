using Application.Abstractions.Mail;

namespace Mail;

internal sealed class MailService : IMailService
{
    private const string FileName = "messageCode.txt";
    
    public async Task<bool> SendMessageAsync(string to, string subject, string body, DateTime expires)
    {
        try
        {
            var message =
                $"{Environment.NewLine}To: {to}{Environment.NewLine}" +
                $"Subject: {subject}{Environment.NewLine}" +
                $"Body: {body}{Environment.NewLine}" +
                $"Expires: {expires}{Environment.NewLine}" +
                $"------------------------------------------{Environment.NewLine}";

            var path = Path.Combine(Directory.GetCurrentDirectory(), FileName);
            await File.AppendAllTextAsync(path, message);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}