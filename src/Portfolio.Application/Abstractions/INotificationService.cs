namespace Portfolio.Application.Abstractions;

/// <summary>Pushes short notifications to the site owner (Telegram in production).</summary>
public interface INotificationService
{
    Task NotifyAsync(string message, CancellationToken cancellationToken = default);
}
