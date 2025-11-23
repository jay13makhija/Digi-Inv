namespace DigiInv.Application.Interfaces;

public interface INotificationService
{
    Task SendOrderConfirmationAsync(string email, int orderId);
}
