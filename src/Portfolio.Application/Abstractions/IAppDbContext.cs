using Microsoft.EntityFrameworkCore;
using Portfolio.Domain.Entities;

namespace Portfolio.Application.Abstractions;

public interface IAppDbContext
{
    DbSet<Profile> Profiles { get; }
    DbSet<SkillCategory> SkillCategories { get; }
    DbSet<Skill> Skills { get; }
    DbSet<Experience> Experiences { get; }
    DbSet<Project> Projects { get; }
    DbSet<BlogPost> BlogPosts { get; }
    DbSet<ContactMessage> ContactMessages { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
