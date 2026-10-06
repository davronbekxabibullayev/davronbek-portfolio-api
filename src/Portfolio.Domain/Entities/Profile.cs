using Portfolio.Domain.Common;

namespace Portfolio.Domain.Entities;

/// <summary>The site owner. A single row.</summary>
public sealed class Profile : AuditableEntity, ITranslatable<ProfileTranslation>
{
    public required string Email { get; set; }
    public string? PhotoUrl { get; set; }
    public string? LinkedInUrl { get; set; }
    public string? GitHubUrl { get; set; }
    public string? TelegramUrl { get; set; }
    public DateOnly CareerStartDate { get; set; }
    public int TeamSize { get; set; }
    public bool OpenToWork { get; set; }

    public ICollection<ProfileTranslation> Translations { get; set; } = [];
}

public sealed class ProfileTranslation : Translation
{
    public Guid ProfileId { get; set; }
    public required string FullName { get; set; }
    public required string Headline { get; set; }
    public required string Tagline { get; set; }
    public required string About { get; set; }
    public required string Location { get; set; }
    public string? CvUrl { get; set; }
}
