using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portfolio.Domain.Entities;

namespace Portfolio.Infrastructure.Persistence.Configurations;

internal sealed class ProfileConfiguration : IEntityTypeConfiguration<Profile>
{
    public void Configure(EntityTypeBuilder<Profile> b)
    {
        b.ToTable("profiles");
        b.Property(x => x.Email).HasMaxLength(200).IsRequired();
        b.Property(x => x.PhotoUrl).HasMaxLength(500);
        b.Property(x => x.LinkedInUrl).HasMaxLength(500);
        b.Property(x => x.GitHubUrl).HasMaxLength(500);
        b.Property(x => x.TelegramUrl).HasMaxLength(500);

        b.HasMany(x => x.Translations).WithOne()
            .HasForeignKey(t => t.ProfileId).OnDelete(DeleteBehavior.Cascade);
    }
}

internal sealed class ProfileTranslationConfiguration : IEntityTypeConfiguration<ProfileTranslation>
{
    public void Configure(EntityTypeBuilder<ProfileTranslation> b)
    {
        b.ConfigureTranslation("profile_translations", nameof(ProfileTranslation.ProfileId));
        b.Property(x => x.FullName).HasMaxLength(150).IsRequired();
        b.Property(x => x.Headline).HasMaxLength(200).IsRequired();
        b.Property(x => x.Tagline).HasMaxLength(400).IsRequired();
        b.Property(x => x.Location).HasMaxLength(150).IsRequired();
        b.Property(x => x.CvUrl).HasMaxLength(500);
    }
}

internal sealed class SkillCategoryConfiguration : IEntityTypeConfiguration<SkillCategory>
{
    public void Configure(EntityTypeBuilder<SkillCategory> b)
    {
        b.ToTable("skill_categories");
        b.HasMany(x => x.Translations).WithOne()
            .HasForeignKey(t => t.SkillCategoryId).OnDelete(DeleteBehavior.Cascade);
        b.HasMany(x => x.Skills).WithOne()
            .HasForeignKey(s => s.SkillCategoryId).OnDelete(DeleteBehavior.Cascade);
    }
}

internal sealed class SkillCategoryTranslationConfiguration : IEntityTypeConfiguration<SkillCategoryTranslation>
{
    public void Configure(EntityTypeBuilder<SkillCategoryTranslation> b)
    {
        b.ConfigureTranslation("skill_category_translations", nameof(SkillCategoryTranslation.SkillCategoryId));
        b.Property(x => x.Name).HasMaxLength(100).IsRequired();
    }
}

internal sealed class SkillConfiguration : IEntityTypeConfiguration<Skill>
{
    public void Configure(EntityTypeBuilder<Skill> b)
    {
        b.ToTable("skills");
        b.Property(x => x.Name).HasMaxLength(100).IsRequired();
        b.HasIndex(x => new { x.SkillCategoryId, x.Name }).IsUnique();
    }
}

internal sealed class ExperienceConfiguration : IEntityTypeConfiguration<Experience>
{
    public void Configure(EntityTypeBuilder<Experience> b)
    {
        b.ToTable("experiences");
        b.Property(x => x.Company).HasMaxLength(150).IsRequired();
        b.Property(x => x.CompanyUrl).HasMaxLength(500);
        b.Ignore(x => x.IsCurrent);

        b.HasMany(x => x.Translations).WithOne()
            .HasForeignKey(t => t.ExperienceId).OnDelete(DeleteBehavior.Cascade);
    }
}

internal sealed class ExperienceTranslationConfiguration : IEntityTypeConfiguration<ExperienceTranslation>
{
    public void Configure(EntityTypeBuilder<ExperienceTranslation> b)
    {
        b.ConfigureTranslation("experience_translations", nameof(ExperienceTranslation.ExperienceId));
        b.Property(x => x.Role).HasMaxLength(150).IsRequired();
        // Highlights -> PostgreSQL text[]
    }
}

internal sealed class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> b)
    {
        b.ToTable("projects");
        b.Property(x => x.Slug).HasMaxLength(120).IsRequired();
        b.HasIndex(x => x.Slug).IsUnique();
        b.Property(x => x.LiveUrl).HasMaxLength(500);
        b.Property(x => x.RepositoryUrl).HasMaxLength(500);
        b.Property(x => x.CoverImageUrl).HasMaxLength(500);

        // Public listing filters on published and orders by featured/sort order.
        b.HasIndex(x => new { x.IsPublished, x.IsFeatured, x.SortOrder });

        b.HasMany(x => x.Translations).WithOne()
            .HasForeignKey(t => t.ProjectId).OnDelete(DeleteBehavior.Cascade);
    }
}

internal sealed class ProjectTranslationConfiguration : IEntityTypeConfiguration<ProjectTranslation>
{
    public void Configure(EntityTypeBuilder<ProjectTranslation> b)
    {
        b.ConfigureTranslation("project_translations", nameof(ProjectTranslation.ProjectId));
        b.Property(x => x.Title).HasMaxLength(200).IsRequired();
        b.Property(x => x.Category).HasMaxLength(100).IsRequired();
        b.Property(x => x.Summary).HasMaxLength(600).IsRequired();
    }
}

internal sealed class BlogPostConfiguration : IEntityTypeConfiguration<BlogPost>
{
    public void Configure(EntityTypeBuilder<BlogPost> b)
    {
        b.ToTable("blog_posts");
        b.Property(x => x.Slug).HasMaxLength(160).IsRequired();
        b.HasIndex(x => x.Slug).IsUnique();
        b.Property(x => x.CoverImageUrl).HasMaxLength(500);
        b.Ignore(x => x.IsPublished);

        // Only published posts are listed, newest first.
        b.HasIndex(x => x.PublishedAt)
            .IsDescending()
            .HasFilter("\"PublishedAt\" IS NOT NULL");

        // Tag filter uses array containment: a GIN index keeps it fast.
        b.HasIndex(x => x.Tags).HasMethod("gin");

        b.HasMany(x => x.Translations).WithOne()
            .HasForeignKey(t => t.BlogPostId).OnDelete(DeleteBehavior.Cascade);
    }
}

internal sealed class BlogPostTranslationConfiguration : IEntityTypeConfiguration<BlogPostTranslation>
{
    public void Configure(EntityTypeBuilder<BlogPostTranslation> b)
    {
        b.ConfigureTranslation("blog_post_translations", nameof(BlogPostTranslation.BlogPostId));
        b.Property(x => x.Title).HasMaxLength(200).IsRequired();
        b.Property(x => x.Excerpt).HasMaxLength(500).IsRequired();
        b.Property(x => x.Content).IsRequired();
    }
}

internal sealed class ContactMessageConfiguration : IEntityTypeConfiguration<ContactMessage>
{
    public void Configure(EntityTypeBuilder<ContactMessage> b)
    {
        b.ToTable("contact_messages");
        b.Property(x => x.Name).HasMaxLength(100).IsRequired();
        b.Property(x => x.Email).HasMaxLength(200).IsRequired();
        b.Property(x => x.Message).HasMaxLength(4000).IsRequired();
        b.Property(x => x.LanguageCode).HasMaxLength(2);
        b.Property(x => x.IpAddress).HasMaxLength(45);

        // Admin inbox: unread first, newest first.
        b.HasIndex(x => new { x.IsRead, x.CreatedAt });
    }
}
