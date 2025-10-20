namespace HisuianArchives.Application.Interfaces;

public interface IEmailService
{
    Task SendPasswordResetEmailAsync(string toEmail, string userName, string resetLink);

    Task SendWelcomeEmailAsync(string toEmail, string userName);
}