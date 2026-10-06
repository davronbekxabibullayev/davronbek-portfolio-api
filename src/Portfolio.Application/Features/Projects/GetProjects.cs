using Microsoft.EntityFrameworkCore;
using Portfolio.Application.Abstractions;
using Portfolio.Application.Common;
using Portfolio.Domain.Common;
using Portfolio.Domain.Entities;

namespace Portfolio.Application.Features.Projects;

public sealed record ProjectListItemDto(
    string Slug,
    string Title,
    string Category,
    string Summary,
    IReadOnlyList<string> Stack,
    string? LiveUrl,
    string? RepositoryUrl,
    string? CoverImageUrl,
    bool IsFeatured);

public sealed record ProjectDetailDto(
    string Slug,
    string Title,
    string Category,
    string Summary,
    IReadOnlyList<string> Stack,
    string? LiveUrl,
    string? RepositoryUrl,
    string? CoverImageUrl,
    bool IsFeatured,
    string? Problem,
    string? Role,
    string? Solution,
    string? Results,
    IReadOnlyList<string> AvailableLanguages);

public sealed class GetProjectsHandler(IAppDbContext db)
{
    public async Task<IReadOnlyList<ProjectListItemDto>> HandleAsync(
        string languageCode, bool featuredOnly, CancellationToken ct)
    {
        var lang = Languages.Normalize(languageCode);

        var query = db.Projects
            .AsNoTracking()
            .Where(p => p.IsPublished);

        if (featuredOnly)
            query = query.Where(p => p.IsFeatured);

        var projects = await query
            .Include(p => p.Translations.Where(t => t.LanguageCode == lang || t.LanguageCode == Languages.Default))
            .OrderByDescending(p => p.IsFeatured)
            .ThenBy(p => p.SortOrder)
            .ToListAsync(ct);

        return projects.Select(p => ToListItem(p, lang)).ToList();
    }

    private static ProjectListItemDto ToListItem(Project p, string lang)
    {
        var t = p.Translate(lang);
        return new ProjectListItemDto(
            p.Slug, t?.Title ?? p.Slug, t?.Category ?? string.Empty, t?.Summary ?? string.Empty,
            p.Stack, p.LiveUrl, p.RepositoryUrl, p.CoverImageUrl, p.IsFeatured);
    }
}

public sealed class GetProjectBySlugHandler(IAppDbContext db)
{
    public async Task<ProjectDetailDto> HandleAsync(string slug, string languageCode, CancellationToken ct)
    {
        var lang = Languages.Normalize(languageCode);

        var p = await db.Projects
            .AsNoTracking()
            .Include(x => x.Translations)
            .FirstOrDefaultAsync(x => x.Slug == slug && x.IsPublished, ct)
            ?? throw new NotFoundException("Project", slug);

        var t = p.Translate(lang) ?? throw new NotFoundException("Project translation", $"{slug}/{lang}");

        return new ProjectDetailDto(
            p.Slug, t.Title, t.Category, t.Summary, p.Stack,
            p.LiveUrl, p.RepositoryUrl, p.CoverImageUrl, p.IsFeatured,
            t.Problem, t.Role, t.Solution, t.Results,
            p.Translations.Select(x => x.LanguageCode).Order().ToList());
    }
}
