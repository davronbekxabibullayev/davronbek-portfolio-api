using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portfolio.Domain.Common;

namespace Portfolio.Infrastructure.Persistence.Configurations;

internal static class TranslationConfiguration
{
    /// <summary>
    /// Shared mapping for a translation table: FK to the owner, a 2-letter language code,
    /// and one row per (owner, language).
    /// </summary>
    public static void ConfigureTranslation<TTranslation>(
        this EntityTypeBuilder<TTranslation> b,
        string table,
        string ownerKey)
        where TTranslation : Translation
    {
        b.ToTable(table);
        b.HasKey(t => t.Id);
        b.Property(t => t.LanguageCode).HasMaxLength(2).IsRequired();
        b.HasIndex(ownerKey, nameof(Translation.LanguageCode)).IsUnique();
    }
}
