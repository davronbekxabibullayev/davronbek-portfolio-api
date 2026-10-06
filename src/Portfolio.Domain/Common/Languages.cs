namespace Portfolio.Domain.Common;

/// <summary>Languages the portfolio content is published in.</summary>
public static class Languages
{
    public const string En = "en";
    public const string Uz = "uz";
    public const string Ru = "ru";

    public const string Default = En;

    public static readonly IReadOnlyList<string> All = [En, Uz, Ru];

    public static bool IsSupported(string? code) =>
        code is not null && All.Contains(code.ToLowerInvariant());

    /// <summary>Returns a supported language code, falling back to <see cref="Default"/>.</summary>
    public static string Normalize(string? code) =>
        IsSupported(code) ? code!.ToLowerInvariant() : Default;
}
