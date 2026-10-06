using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Portfolio.Application.Abstractions;

namespace Portfolio.Infrastructure.Notifications;

public sealed class TelegramOptions
{
    public const string SectionName = "Telegram";

    /// <summary>Bot token from @BotFather. Empty disables notifications.</summary>
    public string? BotToken { get; set; }

    /// <summary>Your personal chat id (send /start to the bot, then call getUpdates).</summary>
    public string? ChatId { get; set; }

    public bool IsConfigured => !string.IsNullOrWhiteSpace(BotToken) && !string.IsNullOrWhiteSpace(ChatId);
}

internal sealed class TelegramNotificationService(
    HttpClient http,
    IOptions<TelegramOptions> options,
    ILogger<TelegramNotificationService> logger) : INotificationService
{
    public async Task NotifyAsync(string message, CancellationToken cancellationToken = default)
    {
        var o = options.Value;
        if (!o.IsConfigured)
        {
            logger.LogInformation("Telegram is not configured; notification skipped");
            return;
        }

        using var response = await http.PostAsJsonAsync(
            $"bot{o.BotToken}/sendMessage",
            new { chat_id = o.ChatId, text = message, disable_web_page_preview = true },
            cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new HttpRequestException($"Telegram sendMessage failed: {(int)response.StatusCode} {body}");
        }
    }
}
