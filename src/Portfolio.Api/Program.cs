using System.Threading.RateLimiting;
using Microsoft.AspNetCore.HttpOverrides;
using Portfolio.Api.Endpoints;
using Portfolio.Api.Infrastructure;
using Portfolio.Application;
using Portfolio.Infrastructure;
using Portfolio.Infrastructure.Persistence.Seed;

var builder = WebApplication.CreateBuilder(args);

Console.Title = "Portfolio API";

builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddHealthChecks();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(o => o.SwaggerDoc("v1", new() { Title = "Portfolio API", Version = "v1" }));

builder.Services.AddOutputCache(o =>
{
    // Public content changes rarely; vary by language and query.
    o.AddPolicy(CachePolicies.PublicContent, p => p
        .Expire(TimeSpan.FromMinutes(5))
        .SetVaryByQuery("lang", "featured", "page", "pageSize", "tag")
        .Tag(CachePolicies.PublicContent));
});

builder.Services.AddRateLimiter(o =>
{
    o.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    o.AddPolicy(RateLimitPolicies.Contact, http =>
        RateLimitPartition.GetFixedWindowLimiter(
            http.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            _ => new FixedWindowRateLimiterOptions { PermitLimit = 5, Window = TimeSpan.FromMinutes(10) }));
});

var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];
builder.Services.AddCors(o => o.AddDefaultPolicy(p => p
    .WithOrigins(allowedOrigins)
    .AllowAnyHeader()
    .AllowAnyMethod()));

// Behind Nginx: trust X-Forwarded-For / X-Forwarded-Proto so rate limiting sees the real client IP.
builder.Services.Configure<ForwardedHeadersOptions>(o =>
{
    o.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    o.KnownNetworks.Clear();
    o.KnownProxies.Clear();
});

var app = builder.Build();

app.UseForwardedHeaders();
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseRateLimiter();
app.UseOutputCache();

app.MapHealthChecks("/health");

var api = app.MapGroup("/api");
api.MapProfileEndpoints();
api.MapSkillEndpoints();
api.MapExperienceEndpoints();
api.MapProjectEndpoints();
api.MapBlogEndpoints();
api.MapContactEndpoints();

if (app.Configuration.GetValue("Database:MigrateOnStartup", true))
{
    using var scope = app.Services.CreateScope();
    await scope.ServiceProvider.GetRequiredService<DbInitializer>().InitializeAsync();
}

app.Run();

// Exposed for WebApplicationFactory in integration tests.
public partial class Program;
