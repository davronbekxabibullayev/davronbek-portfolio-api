using Microsoft.EntityFrameworkCore;
using Portfolio.Application.Abstractions;
using Portfolio.Domain.Common;

namespace Portfolio.Application.Features.Skills;

public sealed record SkillGroupDto(string Category, IReadOnlyList<string> Skills);

public sealed class GetSkillsHandler(IAppDbContext db)
{
    public async Task<IReadOnlyList<SkillGroupDto>> HandleAsync(string languageCode, CancellationToken ct)
    {
        var lang = Languages.Normalize(languageCode);

        var categories = await db.SkillCategories
            .AsNoTracking()
            .Include(c => c.Translations.Where(t => t.LanguageCode == lang || t.LanguageCode == Languages.Default))
            .Include(c => c.Skills)
            .OrderBy(c => c.SortOrder)
            .ToListAsync(ct);

        return categories
            .Select(c => new SkillGroupDto(
                c.Translate(lang)?.Name ?? string.Empty,
                c.Skills.OrderBy(s => s.SortOrder).Select(s => s.Name).ToList()))
            .ToList();
    }
}
