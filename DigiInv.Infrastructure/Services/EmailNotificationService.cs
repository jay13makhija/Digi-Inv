using DigiInv.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace DigiInv.Infrastructure.Services;

public class EmailNotificationService : INotificationService
{
    private readonly ILogger<EmailNotificationService> _logger;

    public EmailNotificationService(ILogger<EmailNotificationService> logger)
    {
        _logger = logger;
    }

    public Task SendOrderConfirmationAsync(string email, int orderId)
    {
        _logger.LogInformation($"[Mock Email] Sending order confirmation to {email} for Order #{orderId}");
        return Task.CompletedTask;
    }
}
