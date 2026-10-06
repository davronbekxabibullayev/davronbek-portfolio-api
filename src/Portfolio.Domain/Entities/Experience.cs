using Portfolio.Domain.Common;

namespace Portfolio.Domain.Entities;

public sealed class Experience : AuditableEntity, ITranslatable<ExperienceTranslation>
{
    public required string Company { get; set; }
    public string? CompanyUrl { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public int SortOrder { get; set; }

    public bool IsCurrent => EndDate is null;

    public ICollection<ExperienceTranslation> Translations { get; set; } = [];
}

public sealed class ExperienceTranslation : Translation
{
    public Guid ExperienceId { get; set; }
    public required string Role { get; set; }
    public List<string> Highlights { get; set; } = [];
}
