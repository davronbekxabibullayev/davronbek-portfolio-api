using Portfolio.Api.Infrastructure;
using Portfolio.Application.Features.Blog;
using Portfolio.Application.Features.Contact;
using Portfolio.Application.Features.Experiences;
using Portfolio.Application.Features.Profile;
using Portfolio.Application.Features.Projects;
using Portfolio.Application.Features.Skills;

namespace Portfolio.Api.Endpoints;

/// <summary>
/// Read-only endpoints used by the public site. Every one takes <c>?lang=en|uz|ru</c>
/// (unknown values fall back to English).
/// </summary>
internal static class PublicEndpoints
{
    public static void MapProfileEndpoints(this RouteGroupBuilder api) =>
        api.MapGet("/profile", (string? lang, GetProfileHandler h, CancellationToken ct) =>
                h.HandleAsync(lang ?? string.Empty, ct))
            .WithTags("Profile")
            .CacheOutput(CachePolicies.PublicContent);

    public static void MapSkillEndpoints(this RouteGroupBuilder api) =>
        api.MapGet("/skills", (string? lang, GetSkillsHandler h, CancellationToken ct) =>
                h.HandleAsync(lang ?? string.Empty, ct))
            .WithTags("Skills")
            .CacheOutput(CachePolicies.PublicContent);

    public static void MapExperienceEndpoints(this RouteGroupBuilder api) =>
        api.MapGet("/experiences", (string? lang, GetExperiencesHandler h, CancellationToken ct) =>
                h.HandleAsync(lang ?? string.Empty, ct))
            .WithTags("Experience")
            .CacheOutput(CachePolicies.PublicContent);

    public static void MapProjectEndpoints(this RouteGroupBuilder api)
    {
        var group = api.MapGroup("/projects").WithTags("Projects");

        group.MapGet("/", (string? lang, bool? featured, GetProjectsHandler h, CancellationToken ct) =>
                h.HandleAsync(lang ?? string.Empty, featured ?? false, ct))
            .CacheOutput(CachePolicies.PublicContent);

        group.MapGet("/{slug}", (string slug, string? lang, GetProjectBySlugHandler h, CancellationToken ct) =>
                h.HandleAsync(slug, lang ?? string.Empty, ct))
            .CacheOutput(CachePolicies.PublicContent);
    }

    public static void MapBlogEndpoints(this RouteGroupBuilder api)
    {
        var group = api.MapGroup("/blog").WithTags("Blog");

        group.MapGet("/", (string? lang, int? page, int? pageSize, string? tag, GetBlogPostsHandler h, CancellationToken ct) =>
                h.HandleAsync(lang ?? string.Empty, page ?? 1, pageSize ?? 10, tag, ct))
            .CacheOutput(CachePolicies.PublicContent);

        group.MapGet("/{slug}", (string slug, string? lang, GetBlogPostBySlugHandler h, CancellationToken ct) =>
                h.HandleAsync(slug, lang ?? string.Empty, ct))
            .CacheOutput(CachePolicies.PublicContent);
    }

    public static void MapContactEndpoints(this RouteGroupBuilder api) =>
        api.MapPost("/contact", async (
                SubmitContactMessageCommand command,
                SubmitContactMessageHandler h,
                HttpContext http,
                CancellationToken ct) =>
            {
                await h.HandleAsync(command, http.Connection.RemoteIpAddress?.ToString(), ct);
                return Results.Accepted();
            })
            .WithTags("Contact")
            .RequireRateLimiting(RateLimitPolicies.Contact)
            .Produces(StatusCodes.Status202Accepted)
            .ProducesValidationProblem()
            .Produces(StatusCodes.Status429TooManyRequests);
}
