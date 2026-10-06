namespace Portfolio.Domain.Common;

/// <summary>Base for per-language content rows (e.g. ProjectTranslation).</summary>
public abstract class Translation : Entity
{
    public required string LanguageCode { get; set; }
}

public interface ITranslatable<TTranslation> where TTranslation : Translation
{
    ICollection<TTranslation> Translations { get; }
}

public static class TranslatableExtensions
{
    /// <summary>
    /// Picks the translation for <paramref name="languageCode"/>, falling back to the default language,
    /// then to any available translation.
    /// </summary>
    public static TTranslation? Translate<TTranslation>(
        this ITranslatable<TTranslation> entity, string languageCode)
        where TTranslation : Translation =>
        entity.Translations.FirstOrDefault(t => t.LanguageCode == languageCode)
        ?? entity.Translations.FirstOrDefault(t => t.LanguageCode == Languages.Default)
        ?? entity.Translations.FirstOrDefault();
}
