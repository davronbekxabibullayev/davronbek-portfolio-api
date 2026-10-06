using Portfolio.Domain.Common;

namespace Portfolio.Domain.Entities;

public sealed class ContactMessage : Entity
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Message { get; set; }
    public string? LanguageCode { get; set; }
    public string? IpAddress { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public bool IsRead { get; set; }
}
