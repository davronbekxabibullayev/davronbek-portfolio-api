using System.Net.Mail;
using Microsoft.Extensions.Logging;
using Portfolio.Application.Abstractions;
using Portfolio.Application.Common;
using Portfolio.Domain.Common;
using Portfolio.Domain.Entities;

namespace Portfolio.Application.Features.Contact;

/// <param name="Website">Honeypot field: hidden in the UI, real users leave it empty.</param>
public sealed record SubmitContactMessageCommand(
    string Name,
    string Email,
    string Message,
    string? LanguageCode,
    string? Website);

public sealed class SubmitContactMessageHandler(
    IAppDbContext db,
    INotificationService notifications,
    TimeProvider clock,
    ILogger<SubmitContactMessageHandler> logger)
{
    public const int NameMaxLength = 100;
    public const int EmailMaxLength = 200;
    public const int MessageMinLength = 10;
    public const int MessageMaxLength = 4000;

    public async Task HandleAsync(SubmitContactMessageCommand command, string? ipAddress, CancellationToken ct)
    {
        // Bots fill every field. Pretend success so they don't retry.
        if (!string.IsNullOrEmpty(command.Website))
        {
            logger.LogInformation("Contact honeypot triggered from {Ip}", ipAddress);
            return;
        }

        Validate(command);

        var message = new ContactMessage
        {
            Name = command.Name.Trim(),
            Email = command.Email.Trim(),
            Message = command.Message.Trim(),
            LanguageCode = Languages.Normalize(command.LanguageCode),
            IpAddress = ipAddress,
            CreatedAt = clock.GetUtcNow(),
        };

        db.ContactMessages.Add(message);
        await db.SaveChangesAsync(ct);

        // The message is already stored; a failed notification must not fail the request.
        try
        {
            await notifications.NotifyAsync(
                $"📩 New message from portfolio\n\n👤 {message.Name}\n✉️ {message.Email}\n🌐 {message.LanguageCode}\n\n{message.Message}",
                ct);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to send contact notification for message {Id}", message.Id);
        }
    }

    private static void Validate(SubmitContactMessageCommand c)
    {
        var errors = new Dictionary<string, string[]>();

        if (string.IsNullOrWhiteSpace(c.Name) || c.Name.Trim().Length > NameMaxLength)
            errors[nameof(c.Name)] = [$"Name is required and must be at most {NameMaxLength} characters."];

        if (string.IsNullOrWhiteSpace(c.Email) || c.Email.Length > EmailMaxLength || !IsEmail(c.Email.Trim()))
            errors[nameof(c.Email)] = ["A valid email address is required."];

        var length = c.Message?.Trim().Length ?? 0;
        if (length < MessageMinLength || length > MessageMaxLength)
            errors[nameof(c.Message)] = [$"Message must be between {MessageMinLength} and {MessageMaxLength} characters."];

        if (errors.Count > 0)
            throw new ValidationException(errors);
    }

    private static bool IsEmail(string value) =>
        MailAddress.TryCreate(value, out var address) && address.Address == value;
}
