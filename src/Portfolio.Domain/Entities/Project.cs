using Portfolio.Domain.Common;

namespace Portfolio.Domain.Entities;

/// <summary>A portfolio case study.</summary>
public sealed class Project : AuditableEntity, ITranslatable<ProjectTranslation>
{
    /// <summary>URL-friendly, unique, language-independent key, e.g. "uds-emergency-dispatch".</summary>
    public required string Slug { get; set; }
    public List<string> Stack { get; set; } = [];
    public string? LiveUrl { get; set; }
    public string? RepositoryUrl { get; set; }
    public string? CoverImageUrl { get; set; }
    public bool IsFeatured { get; set; }
    public bool IsPublished { get; set; }
    public int SortOrder { get; set; }

    public ICollection<ProjectTranslation> Translations { get; set; } = [];
}

public sealed class ProjectTranslation : Translation
{
    public Guid ProjectId { get; set; }
    public required string Title { get; set; }

    /// <summary>Short label shown above the title, e.g. "SaaS · Microservices".</summary>
    public required string Category { get; set; }
    public required string Summary { get; set; }

    // Case study body — Markdown.
    public string? Problem { get; set; }
    public string? Role { get; set; }
    public string? Solution { get; set; }
    public string? Results { get; set; }
}
