using brevo_csharp.Api;
using brevo_csharp.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using brevo_csharp.Model;

namespace HisuianArchives.Infrastructure.Services;

public class BrevoEmailService : IEmailService
{
    private readonly ILogger<BrevoEmailService> _logger;
    private readonly string _apiKey;
    private readonly string _senderName;
    private readonly string _senderEmail;

    public BrevoEmailService(IConfiguration configuration, ILogger<BrevoEmailService> logger)
    {
        _logger = logger;
        _apiKey = configuration["Brevo:ApiKey"] ?? throw new InvalidOperationException("Brevo API Key not configured.");
        _senderName = configuration["Brevo:SenderName"] ?? "Hisuian Archives";
        _senderEmail = configuration["Brevo:SenderEmail"] ?? throw new InvalidOperationException("Brevo Sender Email not configured.");
    }

    public async System.Threading.Tasks.Task SendPasswordResetEmailAsync(string toEmail, string userName, string resetLink)
    {
        var subject = "Redefinição de Senha - Hisuian Archives";
        var htmlContent = $"<html><body><h2>Olá, {userName}!</h2><p>Use o link para resetar sua senha: <a href='{resetLink}'>Resetar Senha</a></p><hr><p>Email enviado com suporte da Brevo©</p></body></html>";

        await SendEmailAsync(toEmail, userName, subject, htmlContent);
    }

    public async System.Threading.Tasks.Task SendWelcomeEmailAsync(string toEmail, string userName)
    {
        var subject = "Bem-vindo ao Hisuian Archives!";
        var htmlContent = $"<html><body><h2>Bem-vindo, {userName}!</h2><p>Obrigado por se registrar.</p></body></html>";

        await SendEmailAsync(toEmail, userName, subject, htmlContent);
    }

    private async System.Threading.Tasks.Task SendEmailAsync(string toEmail, string toName, string subject, string htmlContent)
    {
        var config = new brevo_csharp.Client.Configuration();
        config.ApiKey["api-key"] = _apiKey;

        var apiInstance = new TransactionalEmailsApi(config);

        var sender = new SendSmtpEmailSender(_senderName, _senderEmail);
        var to = new List<SendSmtpEmailTo> { new SendSmtpEmailTo(toEmail, toName) };

        var sendSmtpEmail = new SendSmtpEmail(sender: sender, to: to, subject: subject, htmlContent: htmlContent);

        try
        {
            var result = await apiInstance.SendTransacEmailAsync(sendSmtpEmail);
            _logger.LogInformation("Email sent to {Email}. Subject: '{Subject}'. Message ID: {MessageId}", toEmail, subject, result.MessageId);
        }
        catch (ApiException e)
        {
            _logger.LogError(e, "Brevo API error while sending email to {Email}. Status Code: {StatusCode}, Body: {Body}", toEmail, e.ErrorCode, e.Message);
            throw;
        }
    }
}