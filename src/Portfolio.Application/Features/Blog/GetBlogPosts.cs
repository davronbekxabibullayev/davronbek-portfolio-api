using Microsoft.EntityFrameworkCore;
using Portfolio.Application.Abstractions;
using Portfolio.Application.Common;
using Portfolio.Domain.Common;

namespace Portfolio.Application.Features.Blog;

public sealed record BlogPostListItemDto(
    string Slug,
    string Title,
    string Excerpt,
    IReadOnlyList<string> Tags,
    string? CoverImageUrl,
    DateTimeOffset PublishedAt,
    int ReadingMinutes);

public sealed record BlogPostDto(
    string Slug,
    string Title,
    string Excerpt,
    string Content,
    IReadOnlyList<string> Tags,
    string? CoverImageUrl,
    DateTimeOffset PublishedAt,
    int ReadingMinutes);

internal static class ReadingTime
{
    private const int WordsPerMinute = 200;

    public static int Estimate(string markdown)
    {
        var words = markdown.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries).Length;
        return Math.Max(1, (int)Math.Round(words / (double)WordsPerMinute));
    }
}

public sealed class GetBlogPostsHandler(IAppDbContext db)
{
    public const int MaxPageSize = 50;

    public async Task<PagedResult<BlogPostListItemDto>> HandleAsync(
        string languageCode, int page, int pageSize, string? tag, CancellationToken ct)
    {
        var lang = Languages.Normalize(languageCode);
        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 1, MaxPageSize);

        var query = db.BlogPosts.AsNoTracking().Where(p => p.PublishedAt != null);

        if (!string.IsNullOrWhiteSpace(tag))
            query = query.Where(p => p.Tags.Contains(tag));

        var total = await query.CountAsync(ct);

        var posts = await query
            .Include(p => p.Translations.Where(t => t.LanguageCode == lang || t.LanguageCode == Languages.Default))
            .OrderByDescending(p => p.PublishedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        var items = posts
            .Select(p =>
            {
                var t = p.Translate(lang)!;
                return new BlogPostListItemDto(
                    p.Slug, t.Title, t.Excerpt, p.Tags, p.CoverImageUrl,
                    p.PublishedAt!.Value, ReadingTime.Estimate(t.Content));
            })
            .ToList();

        return new PagedResult<BlogPostListItemDto>(items, page, pageSize, total);
    }
}

public sealed class GetBlogPostBySlugHandler(IAppDbContext db)
{
    public async Task<BlogPostDto> HandleAsync(string slug, string languageCode, CancellationToken ct)
    {
        var lang = Languages.Normalize(languageCode);

        var p = await db.BlogPosts
            .AsNoTracking()
            .Include(x => x.Translations.Where(t => t.LanguageCode == lang || t.LanguageCode == Languages.Default))
            .FirstOrDefaultAsync(x => x.Slug == slug && x.PublishedAt != null, ct)
            ?? throw new NotFoundException("Blog post", slug);

        var t = p.Translate(lang) ?? throw new NotFoundException("Blog post translation", $"{slug}/{lang}");

        return new BlogPostDto(
            p.Slug, t.Title, t.Excerpt, t.Content, p.Tags, p.CoverImageUrl,
            p.PublishedAt!.Value, ReadingTime.Estimate(t.Content));
    }
}
