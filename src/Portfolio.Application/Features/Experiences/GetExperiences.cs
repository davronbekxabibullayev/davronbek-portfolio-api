using Microsoft.EntityFrameworkCore;
using Portfolio.Application.Abstractions;
using Portfolio.Domain.Common;

namespace Portfolio.Application.Features.Experiences;

public sealed record ExperienceDto(
    Guid Id,
    string Company,
    string? CompanyUrl,
    string Role,
    DateOnly StartDate,
    DateOnly? EndDate,
    bool IsCurrent,
    IReadOnlyList<string> Highlights);

public sealed class GetExperiencesHandler(IAppDbContext db)
{
    public async Task<IReadOnlyList<ExperienceDto>> HandleAsync(string languageCode, CancellationToken ct)
    {
        var lang = Languages.Normalize(languageCode);

        var items = await db.Experiences
            .AsNoTracking()
            .Include(e => e.Translations.Where(t => t.LanguageCode == lang || t.LanguageCode == Languages.Default))
            .OrderBy(e => e.SortOrder)
            .ThenByDescending(e => e.StartDate)
            .ToListAsync(ct);

        return items
            .Select(e =>
            {
                var t = e.Translate(lang);
                return new ExperienceDto(
                    e.Id, e.Company, e.CompanyUrl, t?.Role ?? string.Empty,
                    e.StartDate, e.EndDate, e.IsCurrent, t?.Highlights ?? []);
            })
            .ToList();
    }
}
