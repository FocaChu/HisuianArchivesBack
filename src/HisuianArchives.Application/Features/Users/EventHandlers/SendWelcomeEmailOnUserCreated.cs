using MediatR;
using HisuianArchives.Domain.Events;
using Microsoft.Extensions.Logging;

namespace HisuianArchives.Application.Features.Users.EventHandlers;

public class SendWelcomeEmailOnUserCreated : INotificationHandler<UserCreatedEvent>
{
    private readonly IEmailService _emailService;
    private readonly ILogger<SendWelcomeEmailOnUserCreated> _logger;

    public SendWelcomeEmailOnUserCreated(IEmailService emailService, ILogger<SendWelcomeEmailOnUserCreated> logger)
    {
        _emailService = emailService;
        _logger = logger;
    }

    public async Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Sending welcome email to user {Email}", notification.User.Email);
            await _emailService.SendWelcomeEmailAsync(notification.User.Email, notification.User.Name);
            _logger.LogInformation("Welcome email sent successfully to user {Email}", notification.User.Email);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send welcome email to user {Email}", notification.User.Email);
            // Note: We don't rethrow here to avoid breaking the main user creation flow
            // In a production system, you might want to implement a retry mechanism or dead letter queue
        }
    }
}