using Portfolio.Domain.Common;

namespace Portfolio.Domain.Entities;

public sealed class SkillCategory : Entity, ITranslatable<SkillCategoryTranslation>
{
    public int SortOrder { get; set; }

    public ICollection<SkillCategoryTranslation> Translations { get; set; } = [];
    public ICollection<Skill> Skills { get; set; } = [];
}

public sealed class SkillCategoryTranslation : Translation
{
    public Guid SkillCategoryId { get; set; }
    public required string Name { get; set; }
}

/// <summary>Technology names are not translated (".NET 8" is ".NET 8" in every language).</summary>
public sealed class Skill : Entity
{
    public Guid SkillCategoryId { get; set; }
    public required string Name { get; set; }
    public int SortOrder { get; set; }
}
