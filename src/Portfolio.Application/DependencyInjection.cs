using Microsoft.Extensions.DependencyInjection;
using Portfolio.Application.Features.Blog;
using Portfolio.Application.Features.Contact;
using Portfolio.Application.Features.Experiences;
using Portfolio.Application.Features.Profile;
using Portfolio.Application.Features.Projects;
using Portfolio.Application.Features.Skills;

namespace Portfolio.Application;

public static class DependencyInjection
{
    /// <summary>
    /// Registers use-case handlers. Plain classes on purpose: no mediator library
    /// (MediatR went commercial) and nothing to discover at runtime.
    /// </summary>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<GetProfileHandler>();
        services.AddScoped<GetSkillsHandler>();
        services.AddScoped<GetExperiencesHandler>();
        services.AddScoped<GetProjectsHandler>();
        services.AddScoped<GetProjectBySlugHandler>();
        services.AddScoped<GetBlogPostsHandler>();
        services.AddScoped<GetBlogPostBySlugHandler>();
        services.AddScoped<SubmitContactMessageHandler>();

        return services;
    }
}
