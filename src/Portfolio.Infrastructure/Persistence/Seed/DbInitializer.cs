using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Portfolio.Infrastructure.Persistence.Seed;

/// <summary>Applies pending migrations and seeds the initial content on an empty database.</summary>
public sealed class DbInitializer(AppDbContext db, ILogger<DbInitializer> logger)
{
    public async Task InitializeAsync(CancellationToken ct = default)
    {
        if (!db.Database.GetMigrations().Any())
        {
            logger.LogWarning(
                "No EF Core migrations found. Run: dotnet ef migrations add InitialCreate " +
                "-p src/Portfolio.Infrastructure -s src/Portfolio.Api -o Persistence/Migrations");
            return;
        }

        await db.Database.MigrateAsync(ct);

        if (await db.Profiles.AnyAsync(ct))
            return;

        logger.LogInformation("Seeding initial portfolio content");

        db.Profiles.Add(SeedData.Profile());
        db.SkillCategories.AddRange(SeedData.SkillCategories());
        db.Experiences.AddRange(SeedData.Experiences());
        db.Projects.AddRange(SeedData.Projects());

        await db.SaveChangesAsync(ct);
    }
}
