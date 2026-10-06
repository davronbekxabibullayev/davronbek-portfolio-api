using Microsoft.EntityFrameworkCore;
using Portfolio.Application.Abstractions;
using Portfolio.Application.Common;
using Portfolio.Domain.Common;

namespace Portfolio.Application.Features.Profile;

public sealed record ProfileDto(
    string FullName,
    string Headline,
    string Tagline,
    string About,
    string Location,
    string Email,
    string? PhotoUrl,
    string? CvUrl,
    string? LinkedInUrl,
    string? GitHubUrl,
    string? TelegramUrl,
    int YearsOfExperience,
    int TeamSize,
    int ProjectsCount,
    bool OpenToWork);

public sealed class GetProfileHandler(IAppDbContext db, TimeProvider clock)
{
    public async Task<ProfileDto> HandleAsync(string languageCode, CancellationToken ct)
    {
        var lang = Languages.Normalize(languageCode);

        var profile = await db.Profiles
            .AsNoTracking()
            .Include(p => p.Translations.Where(t => t.LanguageCode == lang || t.LanguageCode == Languages.Default))
            .FirstOrDefaultAsync(ct)
            ?? throw new NotFoundException("Profile", "default");

        var t = profile.Translate(lang)
            ?? throw new NotFoundException("Profile translation", lang);

        var projectsCount = await db.Projects.CountAsync(p => p.IsPublished, ct);

        var today = DateOnly.FromDateTime(clock.GetUtcNow().UtcDateTime);
        var years = today.Year - profile.CareerStartDate.Year;
        if (today < profile.CareerStartDate.AddYears(years)) years--;

        return new ProfileDto(
            t.FullName, t.Headline, t.Tagline, t.About, t.Location,
            profile.Email, profile.PhotoUrl, t.CvUrl,
            profile.LinkedInUrl, profile.GitHubUrl, profile.TelegramUrl,
            Math.Max(years, 0), profile.TeamSize, projectsCount, profile.OpenToWork);
    }
}
