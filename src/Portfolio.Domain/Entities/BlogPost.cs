using Portfolio.Domain.Common;

namespace Portfolio.Domain.Entities;

public sealed class BlogPost : AuditableEntity, ITranslatable<BlogPostTranslation>
{
    public required string Slug { get; set; }
    public List<string> Tags { get; set; } = [];
    public string? CoverImageUrl { get; set; }

    /// <summary>Null while the post is a draft.</summary>
    public DateTimeOffset? PublishedAt { get; set; }

    public bool IsPublished => PublishedAt is not null;

    public ICollection<BlogPostTranslation> Translations { get; set; } = [];
}

public sealed class BlogPostTranslation : Translation
{
    public Guid BlogPostId { get; set; }
    public required string Title { get; set; }
    public required string Excerpt { get; set; }

    /// <summary>Post body in Markdown.</summary>
    public required string Content { get; set; }
}
